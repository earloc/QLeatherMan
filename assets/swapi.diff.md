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

