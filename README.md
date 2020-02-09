# QLeatherMan
The ~~swiss-army-knife~~ leatherman when dealing with GraphQL schemas in .NET-land.

## What is it?
QLeatherman, or short qlman, is a dotnet core cli-utility to help deailing with common tasks when GraphQL meets .NET (Core).

Main aspects are:
- easy-to-use from the start (via dotnet-tool)
- great interop with various build-pipelines
- OOTB generation of C#-clients given a GraphQL schema
- maintaining backwards-compatible GraphQL-schemas by providing automated reports of changes between two version of an API (you can, of course, compare two completely unrelated schemas, if that makes any sense to you ^^)

### Examples
#### Generating C#-Clients
invoking the following command will generate a full C# client for a given schema:

```
dotnet qlman generate https://api.spacex.land/graphql
```

> As QLeatherman currently completely relies on [GraphQlClientGenerator] when generating client-classes, head over there for details and don´t forget to leave a 
<a class="github-button" href="https://github.com/Husqvik/GraphQlClientGenerator" data-icon="octicon-star" aria-label="Star Husqvik/GraphQlClientGenerator on GitHub">Star</a> ;)

#### Comparing two (verions of) GraphQL-schema(s)

Invoking the following command will generate a report showing the current differences between the given schemas:

```
dotnet qlman compare https://countries.trevorblades.com/ https://api.spacex.land/graphql/
```

## ToDos
- [ ] publish as dotnet-tool ;)
- [x] generate C#-client based on a given GraphQL schema
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