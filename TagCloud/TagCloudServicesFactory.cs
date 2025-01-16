using System.Drawing;
using Autofac;
using TagCloud.ColorSelectors;
using TagCloud.Excluders;
using TagCloud.TextHandlers;
using TagCloud.WordFilters;
using TagCloudApplication;
using TagCloudTests;
using ColorConverter = TagCloud.Extensions.ColorConverter;
using System.Linq;
using System.Reflection;

namespace TagCloud;

public static class TagCloudServicesFactory
{
    public static IContainer ConfigureService(Options options)
    {
        var builder = new ContainerBuilder();
        
        builder.RegisterType<TagCloudGenerator>().AsSelf().SingleInstance();
        
        builder.Register<ICloudShaper>(provider => SpiralCloudShaper.Create(new Point(0, 0), coefficient: options.Density)).SingleInstance();
        builder.RegisterType<CloudLayouter>().AsSelf().SingleInstance();
        
        builder.Register(provider => new Font(new FontFamily(options.Font), options.FontSize)).As<Font>().InstancePerLifetimeScope();
        builder.RegisterType<TextMeasurer>().AsSelf().SingleInstance();
        
        builder.Register(provider => TagCloudDrawer.Create(
                options.DestinationPath, 
                options.Name, 
                provider.Resolve<Font>(),
                provider.Resolve<IColorSelector>()
            )).As<ICloudDrawer>().SingleInstance();
        
        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.IsAssignableTo<IColorSelector>())
            .As<IColorSelector>()
            .SingleInstance();

        builder.Register<IColorSelector>(context =>
        {
            var colorSelectors = context.Resolve<IEnumerable<IColorSelector>>();
            var selector = colorSelectors.FirstOrDefault(s => s.IsMatch(options));
            
            return selector ?? new ConstantColorSelector(Color.Black);
        }).SingleInstance(); //при ресолве будет использоваться последний зарегистрированный класс

        builder.RegisterType<MyStemWordFilter>().As<IWordFilter>().SingleInstance();
        
        builder.Register(provider => 
            new FileTextHandler(
                stream: File.Open(options.SourcePath, FileMode.Open), 
                filter: provider.Resolve<IWordFilter>()
            )
        ).As<ITextHandler>().SingleInstance();
        
        return builder.Build();
    }

    public static bool ConfigureServiceAndTryGet<T>(Options option, out T value)
    {
        using var container = ConfigureService(option);
        value = container.Resolve<T>();
        return value != null;
    }
}