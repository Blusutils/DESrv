# Contribution guidelines

**DESrv** is **opensource** project what welomes to any contributions. Please follow these guidelines to make them.

## Ways to contribute

### Reports about bugs/vulnerabilies/strange things. Suggestions of new ideas/features

Don't be shy to report about bugs/vulnerabilies/strange things. [Issues](https://github.com/Blusutils/DESrv/issues) page always open to you!
You also can suggest anything about DESrv in the same place.

> **Note**: please use one of existing issue templates.

### Localization

You can suggest translation to your language for Wiki, Documentation and Core on our [SimpleTranslate](https://simpletranslate.net/projects/blusutils-desrv) page (or by creating PR if website offline).

> **Note**: no need to translate into Russian or English. Only correction suggestions.

### Source code

Feel free to create forks and pull requests with improvements. See guide below to learn how to.

#### Prerequesties

* Visual Studio 2022 (17)
* .NET 6.0
* Git (any version)

#### Fork, clone, modify, push, PR

1. Create a new fork of this repository.
2. Clone your fork:

    ```batch
    git clone https://github.com/You/YourForkRepo.git
    ```

3. Modify source code with any changes you want.
4. Test your changes:

    ```batch
    dotnet build
    :: or build solution directly from VS
    ```

5. Push to fork:

    ```batch
    git add .
    :: or any other message
    git commit -m "I've changed something"
    git push origin master
    ```

6. Open PR in original repository.

## Other contribution guidelines

1. Targets of contributions

    * We do not consider contributions related to extensions/addons or DESCEndLib, only Core and PDK (DESCEndLib has its [own repository](https://github.com/Blusutils/DESCEndLib)).

2. Bug reports.

    * Please provide clear and concise (as you can) description of what the bug is, versions of Core and libraries you are using.

    * If bug is able to reproduce, please describe how it can be reproduce (code examples, video, etc.)

    * If it possible, please attach a relevant log output (`*desrv directory*/logs`) and/or screenshots. Additional context that can be useful also welcome.

    > These items can help us understand the problem and fix it faster.

3. Enhancements

    * Please describe your suggestion as clearly and completely as possible. If you are able, add code examples.

    * The enhancements must not harm the DESrv Core and PDK code base. They must also be implementable.

4. Changes in code

    * The pull requests must not harm the DESrv Core and PDK code base. They must also be applicable to current code base. 

    * Please stick to DESrv code styling (our modified Java style instead of classic C# style).
        * Brackets on the same line.
        * Tab size - 4 spaces.
        * Delegates must end with `Delegate`, events with `Event`.
        * Interfaces must starts with `I`, abstract classes with `Abstract`.
        * `PascalCase` for class, struct, interface, namespace, delegate, event, record, property, method; `camelCase` for field.

    ```cs
    // that is recommended
    interface ITest {}
    abstract class AbstractFoo {}
    class FooClass : ITest, AbstractFoo {
        delegate void SomeDelegate();
        public void Foo() {
            if (true) { 
                return;
            }
        }
    }

    // and that isn't
    interface Test {}
    abstract class AbcFoo {}
    class bar_class : Test, AbcFoo {
      delegate void some();
      public void bAR()
      {
        if (true)
        {
          return;
        }
      }
    }
    ```

5. Localization
    * coming soon because SimpleTranslate not done yet
