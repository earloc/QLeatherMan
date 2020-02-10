# QLeatherMan
The ~~swiss-army-knife~~ multi-tool of choice when dealing with GraphQL schemas in .NET-land.

![.NET Core](https://github.com/earloc/QLeatherMan/workflows/.NET%20Core/badge.svg)

## What is it?
QLeatherman, or short qlman, is a dotnet core cli-utility to help deailing with common tasks when GraphQL meets .NET (Core).

Main aspects are:
- easy-to-use from the start (via dotnet-tool)
- great interop with various build-pipelines
- OOTB generation of C#-clients given a GraphQL schema
- maintaining backwards-compatible GraphQL-schemas by providing automated reports of changes between two version of an API (you can, of course, compare two completely unrelated schemas, if that makes any sense to you ^^)

## How to get it?

### dotnet global tool
Run the follwoing command to download QLeatherman as a global dotnet-tool

```
dotnet tool install --global QLeatherman
```

### Examples
#### Generating C#-Clients
invoking the following command will generate a full C# client for a given schema:

```
dotnet qlman generate https://swapi.apis.guru/
```

> As QLeatherman currently completely relies on [GraphQlClientGenerator] when generating client-classes, head over there for details and don´t forget to leave a 
<a class="github-button" href="https://github.com/Husqvik/GraphQlClientGenerator" data-icon="octicon-star" aria-label="Star Husqvik/GraphQlClientGenerator on GitHub">Star</a> ;)

#### Comparing two (verions of) GraphQL-schema(s)

Invoking the following command will generate a report showing the current differences between the given schemas:

```
dotnet qlman compare https://swapi.apis.guru/ https://api.spacex.land/graphql/
```
> the sample comparison above does not actually make any sense, but showcases the output of differing schemas

During comparison, the used schemas are cached locally in **left**.json and **rigt**.json, respectiviley.

To use these local cached version, run the following command

```
dotnet qlman compare left.json https://swapi.apis.guru/
```

to compare the local file **left.json** with the remote-schema **https://swapi.apis.guru/**.
> Urls and files can be mixed and matched as needed.


## ToDos
following is a list of pending features, ordered per context, not per priority 

- [x] publish as dotnet-tool ;)
  - [ ] automate publishing
- [x] generate C#-client based on a given GraphQL schema
  - > currently using [GraphQlClientGenerator]
- [x] generate difference report given two GraphQL schemas
  - [x] support local files for analyzing evolvement of remote schemas
  - [x] show differences in types
    - [x] added types
    - [x] removed types
    - [ ] deprecated types
    - [x]  modified types
      - [x] added fields
      - [x] removed fields
      - [ ] deprecated fields
      - [ ] modified fields
        - [ ] type changed
        - [ ] nullability changed
        - [ ] other?
  - [ ] differences in queries
    - [ ] added arguments
    - [ ] removed arguments
    - [ ] deprecated arguments
  - [ ] differences in mutations
    - [ ] added arguments
    - [ ] removed arguments
    - [ ] deprecated arguments
  - [ ] differences in subscriptions
  - [ ] differences in directives
  - [ ] more?
- [ ] batching support to be used by e.g. build-pipelines
- [ ] customizable reports and formats
  - [ ] markdown
  - [ ] others?
- [ ] provide better examples on use-cases of 'compare'


## Cotributions welcome:
- file an issue
- fork the repository
- make the changes
- submit a pull-request
- hope for the best ;)

## Thanks to others
- [GraphQlClientGenerator] (MIT-license)
- [CommandLineParser] (MIT-license)


[GraphQlClientGenerator]:https://github.com/Husqvik/GraphQlClientGenerator
[CommandLineParser]:https://github.com/commandlineparser/commandline