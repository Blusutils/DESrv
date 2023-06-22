# Contribution ways and guidelines

**DESrv** is **opensource** project what welomes to any contributions. Please follow these guidelines to make them.

## Ways to contribute

This section contains some hints about what you can contribute.

### Suggestions or bug reports

Don't be shy to report about bugs/vulnerabilies/strange things on the [Issues](https://github.com/Blusutils/DESrv/issues) page.
You also can suggest anything about DESrv in the same place.

> **Note**: please use one of existing issue templates.

### Localization files edits

You can suggest translation to your language for Documentation or Core by creating pull request.

> **Note**: please affect only one directory in "translation pull requests" - `translations`. PRs that points as "translation pull requests" but has requesting changes to files outside this directory will be rejected on review.

### Source code

Feel free to create forks and pull requests with improvements. See guide below to learn how to.

#### Prerequesties

* Visual Studio 2022 (17) (highly recommended)
* .NET SDK 7.0 or above
* Git (any version)

1. Create a new fork of this repository.
2. Clone your fork:

    ```bash
    git clone https://github.com/You/YourForkRepo.git
    ```

3. Modify source code with any changes you want.
4. Test your changes:

    ```bash
    # or perform tests directly from VS
    dotnet test DESrv.sln
    ```

6. Push to fork:

    ```bash
    git add . # if files is untracked
    git commit -m "Commit message" # it is highly recommended to separate your work to certain commits
    git push origin master
    ```

7. Open PR in original repository. Don't forget to describe your changes - it will speed up the PR review process.

## Contribution guidelines

### 1. Targets of contributions

* We do not consider contributions related to extensions/addons (except `DESrv.InternalPlugin`, which is bundled to this repo).
* If contribution affects more than one project in DESrv repo, it is recommended to separate changes to several commits or pull requests.

### 2. Bug reports

* Please provide clear and concise (as you can) description of what the bug is, versions of Core and libraries you are using.

* If bug is able to reproduce, please describe how it can be reproduce (code examples, videos, screenshots, etc.)

* If it possible, please attach a relevant log output (`*desrv directory*/logs`) and/or screenshots.

* Additional context that can be useful also welcome.

### 3. Enhancements

* Please describe your suggestion as clearly and completely as possible.

* If you are able, add code examples.
  * The enhancements must not harm the DESrv code base.

* Enhancement must ofcourse be implementable.

### 4. Translations

* Consider changing only the `translations` directory and creating a ONE new JSON file with your strings.

* If suggested strings is not valid or contains "bad" contents, pull request will be closed.

* And if you are adding a new language...
  * Please check that the language alphabet is printable by most common console hosts.
  * If language is not exist, pull request will be closed.

### 5. Changes in code

* The pull requests must not harm the DESrv Core and PDK code base. They must also be applicable to current code base.

* Please stick to DESrv code styling (our modified Mono style):
  * Brackets on the same line.
  * Tab size - 4 spaces.
  * Delegates must end with `Delegate`, events with `Event`.
  * Interfaces must starts with `I`, abstract classes with `Abstract`.
  * `camelCase` for field; `PascalCase` for anything else.
  * Example:

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
