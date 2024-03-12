# RockLib.Secrets.Aws Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 3.0.0-alpha.1 - 2024-03-12

#### Changed
- Removed support for .NET Core 3.1 and added .NET 8.
- Updated all NuGet package references to the latest versions.

## 2.0.2 - 2023-04-11

#### Fixed
- Changed the `secretKey` and `secretsManager` parameters to the `AwsSecret` constructor so configuration only requires two values.

## 2.0.1 - 2022-07-18

#### Changed
- Updated the following package references to their latest versions:
  - `RockLib.Secrets` - `2.0.2`
  - `AWSSDK.SecretsManager` - `3.7.2.65`

## 2.0.0 - 2022-04-14

#### Added
- Added `.editorconfig` and `Directory.Build.props` files to ensure consistency.

#### Changed
- Supported targets: net6.0, netcoreapp3.1, and net48.
- Updated all NuGet package dependencies to their latest versions:
  - `Microsoft.SourceLink.GitHub` - `1.1.1`
  - `AWSSDK.SecretsManager` - `3.7.2.29`
  - `RockLib.Secrets` - `3.0.0`
- As the package now uses nullable reference types, some method parameters now specify if they can accept nullable values.
- The static field, `AwsSecret.DefaultSecretsManager`, has been removed. Typically this would just end up being an instance of `AmazonSecretsManagerClient`. This also means that `AwsConfigurationBuilderExtensions.SetAmazonSecretsManager` is no longer necessary and has been deleted. If you need to create an instance of `AwsSecret`, you can use an instance of `AmazonSecretsManagerClient` to pass to the `secretsManager` parameter. Having this manager type resolved through a registration in your IoC container is the ideal solution for this.

## 1.0.8 - 2021-08-12

#### Changed

- Changes "Quicken Loans" to "Rocket Mortgage".
- Updates RockLib.Secrets to latest version, [2.0.6](https://github.com/RockLib/RockLib.Secrets/blob/main/RockLib.Secrets/CHANGELOG.md#206---2021-08-12).
- Updates AWSSDK.SecretsManager to latest version, [3.7.1.4](https://github.com/aws/aws-sdk-net/blob/master/SDK.CHANGELOG.md#37950-2021-08-12-1814-utc).
- Updates Newtonsoft.Json to latest version, [13.0.1](https://github.com/JamesNK/Newtonsoft.Json/releases/tag/13.0.1).

## 1.0.7 - 2021-05-06

#### Changed

- Updates RockLib.Secrets package to latest versions, which include SourceLink.

## 1.0.6 - 2021-04-22

#### Added

- Adds SourceLink to nuget package.

----

**Note:** Release notes in the above format are not available for earlier versions of
RockLib.Secrets. What follows below are the original release notes.

----

## 1.0.5

Adds net5.0 target.

## 1.0.4

Adds icon to project and nuget package.

## 1.0.3

Updates to align with nuget conventions.

## 1.0.2

Updates RockLib.Secrets package to latest version.

## 1.0.1

Adds support for plaintext aws secrets.

## 1.0.0

Initial release.
