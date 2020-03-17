# Freja eID Client - csharp
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

Freja eID Client is a C# client library aimed to ease integration of relying party back-end systems with Freja eID Relying Party API.

## Features
This client library provides a set of classes, interface and utility methods designed for accomplishing one of the following use cases:

* Authentication Services API Client
  + initiation of an authentication request
  + fetching a single authentication result based on authentication reference
  + fetching multiple authentication results
  + cancelling authentication request
  
## Examples
### Init connection to API (test environment)
```c#
SslSettings sslSettings = SslSettings.Create("/path/to/keystore.pkcs12", "SuperSecretKeystorePassword", "/path/to/server/certificate.crt");
```
### Init, monitor and cancel authentication request
Create authentication client
```c#
IAuthenticationClient authenticationClient = AuthenticationClient.Create(sslSettings, FrejaEnvironment.TEST).Build<AuthenticationClient>();
```
Initiate request
```c#
InitiateAuthenticationRequest request = InitiateAuthenticationRequest.CreateDefaultWithEmail("email@example.com");
string reference = authenticationClient.Initiate(request);
```
Poll for request
```c#
int maxWaitingTimeInSeconds = 30;
AuthenticationResult result = authenticationClient.PollForResult(AuthenticationResultRequest.Create(reference), maxWaitingTimeInSeconds);
```
Cancel request
```c#
authenticationClient.Cancel(CancelAuthenticationRequest.Create(reference));
```

