# QLeatherMan
The ~~swiss-army-knife~~ leatherman when dealing with GraphQL schemas in .NET-land.

## What is it?
QLeatherman, or short qlman, is a dotnet core cli-utility to help deailing with common tasks when GraphQL meets .NET (Core)

Main aspects are:
- easy-to-use from the start (via dotnet-tool)
- great interop with various build-pipelines
- OOTB generation of C#-clients given a GraphQL schemas
- maintaining backwards-compatible GraphQL-schemas by providing automated reports of changes between two version of an API (you can of course diff two completely unrelated schemas, if that makes sense to you ^^)

### Examples
#### generating C#-Clients
invoking the following command will generate a full C# client for a given schema:

```
dotnet qlman generate https://api.spacex.land/graphql
```

#### comparing two (verions) of a GraphQL-schema

Invoking the following command will generate a report showing the current differences between the iven schemas:

```
dotnet qlman compare https://countries.trevorblades.com/
```

> As QLeatherman currently completely relies on [GraphQlClientGenerator] when generating client-classes atm, head over there for details and don´t forget to leave a 
<a class="github-button" href="https://github.com/Husqvik/GraphQlClientGenerator" data-icon="octicon-star" aria-label="Star Husqvik/GraphQlClientGenerator on GitHub">Star</a> ;)

## ToDos
- [ ] actually publish as dotnet-tool ;)
- [x] generate C#-client based on a GraphQL schema
  - > currently using [GraphQlClientGenerator]
- [x] generate difference report given two GraphQL schemas
  - [ ] support local files for analyzing evolvement of remote schemas
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
  - [ ] differnces in mutations
    - [ ] added arguments
    - [ ] removed arguments
    - [ ] deprecated arguments
- [ ] batching support to be used by e.g. build-pipelines
- [ ] customizable reports
  - [ ] markdown
  - [ ] others?


## Thanks to others
- [GraphQlClientGenerator] (MIT-license)
- [CommandLineParser] (MIT-license)


[GraphQlClientGenerator]:https://github.com/Husqvik/GraphQlClientGenerator
[CommandLineParser]:https://github.com/commandlineparser/commandline