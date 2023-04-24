using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Localization;

namespace Blusutils.DESrv.Tests.L10nTests;

/// <summary>
/// Localization implementation tests
/// </summary>
public class LocaleTests : Tests {

    Localizer localizerStrict = new() { CurrentLocale = "en", Strict = true };
    Localizer localizerNonStrict = new() { CurrentLocale = "en", Strict = false };

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

    [Test(Description = "Test getting an invalid key in strict localizer")]
    public void TestInvalidKey() {
        Assert.Throws<LocaleKeyException>(() => localizerStrict.Translate("abcdef"));
    }

    [Test(Description = "Test getting a valid key in strict localizer")]
    public void TestValidKey() {
        Assert.That(localizerStrict.Translate("anKey"), Is.EqualTo("test value"));
    }

    [Test(Description = "Test getting a nested key in strict localizer")]
    public void TestValidKeyNestedOnce() {
        Assert.That(localizerStrict.Translate("nested.inner1"), Is.EqualTo("test inner"));
    }

    [Test(Description = "Test getting a double nested key in strict localizer")]
    public void TestValidKeyNestedTwice() {
        Assert.That(localizerStrict.Translate("nested.moreNested.last"), Is.EqualTo("test last"));
    }


    [Test(Description = "Test getting an invalid key in non-strict localizer")]
    public void TestInvalidKeyUnstrict() {
        Assert.That(localizerNonStrict.Translate("abcdef"), Is.Empty);
    }

    [Test(Description = "Test getting an valid key in non-strict localizer")]
    public void TestValidKeyUnstrict() {
        Assert.That(localizerNonStrict.Translate("anKey"), Is.EqualTo("test value"));
    }

    [Test(Description = "Test getting a nested key in non-strict localizer")]
    public void TestValidKeyNestedOnceUnstrict() {
        Assert.That(localizerNonStrict.Translate("nested.inner1"), Is.EqualTo("test inner"));
    }

    [Test(Description = "Test getting a double nested key in non-strict localizer")]
    public void TestValidKeyNestedTwiceUnstrict() {
        Assert.That(localizerNonStrict.Translate("nested.moreNested.last"), Is.EqualTo("test last"));
    }
}