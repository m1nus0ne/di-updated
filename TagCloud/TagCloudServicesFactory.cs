using System.Drawing;
using Autofac;
using TagCloud.ColorSelectors;
using TagCloud.Excluders;
using TagCloud.TextHandlers;
using TagCloud.WordFilters;
using TagCloudApplication;
using TagCloudTests;
using ColorConverter = TagCloud.Extensions.ColorConverter;

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
        
        if (options.ColorScheme == "gray")
            builder.RegisterType<RandomColorSelector>().As<IColorSelector>().SingleInstance();
        if (options.ColorScheme == "random")
            builder.RegisterType<GrayScaleColorSelector>().As<IColorSelector>().SingleInstance();
        else if (ColorConverter.TryConvert(options.ColorScheme, out var color))
            builder.Register(provider => new ConstantColorSelector(color)).As<IColorSelector>().SingleInstance();
        else
            builder.Register(provider => new ConstantColorSelector(Color.Black)).As<IColorSelector>().SingleInstance();

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