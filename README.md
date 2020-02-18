# QLeatherMan
The ~~swiss-army-knife~~ multi-tool of choice when dealing with GraphQL schemas in .NET-land.

![.NET Core](https://github.com/earloc/QLeatherMan/workflows/.NET%20Core/badge.svg) ![NuGet](https://img.shields.io/nuget/v/QLeatherMan)

## What is it?
QLeatherman, or short qlman, is a dotnet core command-line-interface to help deailing with common tasks when GraphQL meets .NET (Core), especially useful for authors of GraphQl-schemas.

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

### Verbs

QleatherMan uses a verb-style CLI, similar to git or other popular CLIs out there.
To show some help about the possible verbs and it´s parameters, just ommit any parameters

#### generate
Invoking the _generate_ command will yield a full C# client for a given schema:

```
dotnet qlman generate https://swapi.apis.guru/
```

> As QLeatherman currently completely relies on [GraphQlClientGenerator] when generating client-classes, head over there for details and don´t forget to leave a 
<a class="github-button" href="https://github.com/Husqvik/GraphQlClientGenerator" data-icon="octicon-star" aria-label="Star Husqvik/GraphQlClientGenerator on GitHub">Star</a> ;)


per default, deprecated elements are not subject to generation of C#-pendants
to enable generation of deprecated elements, supply the `-d` switch:

```
dotnet qlman generate https://swapi.apis.guru/ -d
```

#### compare

The _compare_-command will generate a report showing the current differences between two given schemas.


```
dotnet qlman compare https://swapi.apis.guru/ https://api.spacex.land/graphql/
```

> the sample comparison above does not actually make any sense, but showcases the output of differing schemas

During comparison, the used schemas are cached locally in **left**.qlman.json and **rigt**.qlman.json, respectiviley.

To use one of these cached version, just replace one of the uris in the compare-command

```
dotnet qlman compare left.qlman.json https://swapi.apis.guru/
```

> this will compare the local file **left.qlman.json** with the remote-schema **https://swapi.apis.guru/**.

> Urls and files can be mixed and matched as needed.

> files ending with qlman.json are treated as if QLeatherMan itself serialized the content. For any other extension, QLeatherMan will fall back to use [GraphQlGenerator]'s implementation.

#### examples

Based on the widely used reference-api based on star-wars, let´s consider we have the schemas of two version of the same API.
> the below shown differences are not reflecting actual evolvement of the api, but are prepared to better showcase the use of the compare-command
> we simulate evolvement by removing all 'Planet'-related types, mutations, etc. from v1, as well as some fields on other types to simulate a hard breaking-change in API-surface.
> you can find the used schemas in the asset-folder within this repo

the output of 
```
dotnet compare swapi.apis.guru.v1.qlman.json swapi.apis.guru.v2.qlman.json -o diff.md
```

would result in [this sample-report]

```
# Differences between
- ..\..\..\..\assets\swapi.apis.guru.v1.qlman.json
- ..\..\..\..\assets\swapi.apis.guru.v2.qlman.json
## legend
- (+) ->   addition
- (-) -> ~~removal~~
- (#) -> **modification**
- (~) -> __deprecation__ (not implemented)
- ! -> non-nullable
- ? -> nullable

## Types
- (+)   Planet  
- (+)   PlanetResidentsConnection  
- (+)   PlanetResidentsEdge  
- (+)   PlanetFilmsConnection  
- (+)   PlanetFilmsEdge  
- (+)   FilmCharactersEdge  
- (+)   FilmPlanetsConnection  
- (+)   FilmPlanetsEdge  
- (+)   PlanetsConnection  
- (+)   PlanetsEdge  
- (#) **Root**
  - (+) allPlanets : PlanetsConnection?
  - (+) planet : Planet?

- (#) **Film**
  - (+) planetConnection : FilmPlanetsConnection?

- (#) **Species**
  - (+) Description_fka_Name : String?
  - (+) homeworld : Planet?
  -  (-) ~~name : String?~~
```


## ToDos
following is a list of pending features, ordered per context, not per priority 

- [x] publish as dotnet-tool ;)
  - [x] automate publishing
- [x] generate C#-client based on a given GraphQL schema
  - > currently using [GraphQlClientGenerator]
- [x] generate difference report given two GraphQL schemas
  - [x] support local files for analyzing evolvement of remote schemas
  - [x] write message to stderr when comparison indicates a breaking-change
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
[this sample-report]:assets/swapi.diff.md