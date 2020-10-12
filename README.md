# Symu.OrgMod as Organizational modeling

``Develop your own application based on organizational modeling<br>
``Symu.OrgMod`` is part of ``Symu Suite``, for organizational modeling, analysis and simulating.
``Symu.OrgMod`` is a core of organizational graph modeling library, written in C#.
It implements agnostic organizations as social groups to target the most general use cases.

Some useful links:
* [Website : symu.org](https://symu.org/)
* [Documentation : docs.symu.org](http://docs.symu.org/)
* [Code : github.symu.org](http://github.symu.org/)
* [Issues : github.symu.org/issues](http://github.symu.org/issues/)
* [Twitter : symuorg](https://twitter.com/symuorg)

## How it works

``Symu.OrgMod`` models organization as a graph of specific artefacts such as :

* Actor
* Belief, Knowledge
* Event
* Organization
* Resource
* Role
* Rask
* Task

## What it is

It creates a graph of organizational artifacts statistical and graphical (not yet available) to model an organization.

## Why open source

Because we believe that such a framework is valuable for organizations and academics, because there are few c # frameworks available.

## Getting Started
The main project is [Symu.OrgMod](https://github.com/lmorisse/Symu.OrgMod/tree/master/sourceCode/SymuOrgMod). This is the framework you'll use to build your own application.
There isn't GUI mode yet.

### Installing


### Building

``Symu.OrgMod`` is built upon different repositories. We don't use git submodules. So that, to build Symu.OrgMod and its examples solutions, you'll need to check the dependencies manually.

#### Symu.DNA dependencies
To build Symu you have to add the Symu.Common.dll as a dependency. You find this library in the [Symu.Common](https://github.com/lmorisse/Symu.Common/releases/latest) and [Symu.DNA](https://github.com/lmorisse/Symu.DNA/releases/latest) repositories.

#### External dependencies

### Running

As it is a core library, you can't run ``Symu.OrgMod`` as is.

## Contributors

See the list of [CONTRIBUTORS](CONTRIBUTORS.md) who participated in this project.

## Contributing

Please read [CONTRIBUTING](CONTRIBUTING.md) for details on how you can contribute and the process for contributing.

## Code of conduct

Please read [CODE_OF_CONDUCT](CODE_OF_CONDUCT.md) for details on our code of conduct if you want to contribute.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/lmorisse/Symu/releases). 

## License

This project is licensed under the GNU General Public License v2.0 - see the [LICENSE](LICENSE) file for details

## Support

Paid consulting and support options are available from the corporate sponsors. See [Symu services](https://symu.org/services/).

## Integration

Symu.OrgMod is used in projects:
- [``Symu.DNA``](https://github.com/lmorisse/Symu.DNA): a framework for static and dynamic organization network analysis.
- [``Symu``](http://github.symu.org/): a multi-agent system, time based with discrete events, for the co-evolution of agents and socio-cultural environments.
- [``Symu.biz``](http://symu.biz): an enterprise level implementation of ``Symu``
