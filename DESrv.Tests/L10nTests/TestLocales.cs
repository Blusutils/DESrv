using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Localization;

namespace Blusutils.DESrv.Tests.L10nTests;

/// <summary>
/// Localization implementation tests
/// </summary>
public class LocaleTests : Tests {

    LocalizationProvider localizerStrict = new() { CurrentLocale = "en", Strict = true };
    LocalizationProvider localizerNonStrict = new() { CurrentLocale = "en", Strict = false };

    [OneTimeSetUp]
    public void Setup() {
        // localization mapping to be used in tests
        var json = @"
{
    ""anKey"": ""test value"",
    ""nested"": {
        ""inner1"": ""test inner"",
        ""moreNested"": {
            ""last"": ""test last""
        }
    }
}
";
        localizerStrict.LoadFromString(json, "en");
        localizerNonStrict.LoadFromString(json, "en");
    }

    [Test(Description = "Get an invalid key in strict localizer")]
    public void InvalidKey() {
        Assert.Throws<LocaleKeyException>(() => localizerStrict.Translate("abcdef"));
    }

    [Test(Description = "Get a valid key in strict localizer")]
    public void ValidKey() {
        Assert.That(localizerStrict.Translate("anKey"), Is.EqualTo("test value"));
    }

    [Test(Description = "Get a nested key in strict localizer")]
    public void ValidKeyNestedOnce() {
        Assert.That(localizerStrict.Translate("nested.inner1"), Is.EqualTo("test inner"));
    }

    [Test(Description = "Get a double nested key in strict localizer")]
    public void ValidKeyNestedTwice() {
        Assert.That(localizerStrict.Translate("nested.moreNested.last"), Is.EqualTo("test last"));
    }


    [Test(Description = "Get an invalid key in non-strict localizer")]
    public void InvalidKeyUnstrict() {
        Assert.That(localizerNonStrict.Translate("abcdef"), Is.Empty);
    }

    [Test(Description = "Get an valid key in non-strict localizer")]
    public void ValidKeyUnstrict() {
        Assert.That(localizerNonStrict.Translate("anKey"), Is.EqualTo("test value"));
    }

    [Test(Description = "Get a nested key in non-strict localizer")]
    public void ValidKeyNestedOnceUnstrict() {
        Assert.That(localizerNonStrict.Translate("nested.inner1"), Is.EqualTo("test inner"));
    }

    [Test(Description = "Get a double nested key in non-strict localizer")]
    public void ValidKeyNestedTwiceUnstrict() {
        Assert.That(localizerNonStrict.Translate("nested.moreNested.last"), Is.EqualTo("test last"));
    }
}