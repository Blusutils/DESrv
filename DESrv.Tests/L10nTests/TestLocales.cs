using Blusutils.DESrv.Configuration;
using Blusutils.DESrv.Localization;

namespace Blusutils.DESrv.Tests.L10nTests;

public class LocaleTests : Tests {
    Localizer localizerStrict = new() { CurrentLocale = "en", Strict = true };
    Localizer localizerNonStrict = new() { CurrentLocale = "en", Strict = false };
    int i = 0;

    [OneTimeSetUp]
    public void Setup() {
        var json = """
{
    "anKey": "test value",
    "nested": {
        "inner1": "test inner",
        "moreNested": {
            "last": "test last"
        }
    }
}
""";
        localizerStrict.LoadFromString(json, "en");
        localizerNonStrict.LoadFromString(json, "en");
    }

    [Test]
    public void TestInvalidKey() {
        Assert.Throws<LocaleKeyException>(() => localizerStrict.Translate("abcdef"));
    }
    [Test]
    public void TestValidKey() {
        Assert.That(localizerStrict.Translate("anKey"), Is.EqualTo("test value"));
    }

    [Test]
    public void TestValidKeyNestedOnce() {
        Assert.That(localizerStrict.Translate("nested.inner1"), Is.EqualTo("test inner"));
    }
    [Test]
    public void TestValidKeyNestedTwice() {
        Assert.That(localizerStrict.Translate("nested.moreNested.last"), Is.EqualTo("test last"));
    }
    [Test]
    public void TestInvalidKeyUnstrict() {
        Assert.That(localizerNonStrict.Translate("abcdef"), Is.Empty);
    }
    [Test]
    public void TestValidKeyUnstrict() {
        Assert.That(localizerNonStrict.Translate("anKey"), Is.EqualTo("test value"));
    }

    [Test]
    public void TestValidKeyNestedOnceUnstrict() {
        Assert.That(localizerNonStrict.Translate("nested.inner1"), Is.EqualTo("test inner"));
    }
    [Test]
    public void TestValidKeyNestedTwiceUnstrict() {
        Assert.That(localizerNonStrict.Translate("nested.moreNested.last"), Is.EqualTo("test last"));
    }
}