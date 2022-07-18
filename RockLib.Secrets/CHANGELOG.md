# RockLib.Secrets Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 3.0.1 - 2022-07-18

#### Changed
- Updated the `RockLib.Configuration.ObjectFactory` package reference to the latest version (`2.0.2`).

## 3.0.0 - 2022-04-12

#### Added
- Added `.editorconfig` and `Directory.Build.props` files to ensure consistency.

#### Changed
- Supported targets: net6.0, netcoreapp3.1, and net48.
- As the package now uses nullable reference types, some method parameters now specify if they can accept nullable values.

## 2.0.6 - 2021-08-12

#### Changed

- Changes "Quicken Loans" to "Rocket Mortgage".
- Updates RockLib.Configuration to latest version, [2.5.3](https://github.com/RockLib/RockLib.Configuration/blob/main/RockLib.Configuration/CHANGELOG.md#253---2021-08-11).
- Updates RockLib.Configuration.ObjectFactory to latest version, [1.6.9](https://github.com/RockLib/RockLib.Configuration/blob/main/RockLib.Configuration.ObjectFactory/CHANGELOG.md#169---2021-08-11).

## 2.0.5 - 2021-05-06

#### Changed

- Updates RockLib.Configuration and RockLib.Configuration.ObjectFactory packages to latest versions, which include SourceLink.

## 2.0.4 - 2021-04-22

#### Added

- Adds SourceLink to nuget package.

----

**Note:** Release notes in the above format are not available for earlier versions of
RockLib.Secrets. What follows below are the original release notes.

----

## 2.0.3

Adds net5.0 target.

## 2.0.2

Adds icon to project and nuget package.

## 2.0.1

Updates to align with nuget conventions.

## 2.0.0

- Made configuration extension methods, sources, & providers more idiomatic.
- Added periodic reloading of secrets.
- Added exception handler for when `ISecret.GetValue()` throws an exception.
- Removed the `ISecretsProvider` interface.
- Renamed `ISecret.Key` to `ConfigurationKey`.
- Updated configuration. Instead of the `RockLib_Secrets` / `RockLib.Secrets` composite section defining an `ISecretsProvider`, the section defines one or more instances of `ISecret`.

## 1.0.2

The parameterless overload of the AddRockLibSecrets extension method throws an exception if the configuration does not define a secrets provider in the RockLib_Secrets / RockLib.Secrets composite configuration section or if a secrets provider cannot be created due to invalid configuration.

## 1.0.1

- Adds support for rocklib_secrets config section.
- Adds ConfigSection attribute for the Rockifier tool.

## 1.0.0

Initial release.
