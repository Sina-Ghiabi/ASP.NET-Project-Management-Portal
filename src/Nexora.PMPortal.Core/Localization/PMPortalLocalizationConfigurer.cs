using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Nexora.PMPortal.Localization
{
    public static class PMPortalLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(PMPortalConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(PMPortalLocalizationConfigurer).GetAssembly(),
                        "Nexora.PMPortal.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
