# ProceduralPlanetGeneration
 Four different planet types; Gas Giant, Earth-like, Rocky, and Cracked
 
 ## Gas Giant
 Gas Giants are planets similar to Jupiter and Neptune in our own solar system, their distinct characteristics are large coloured ‘bands’ based on a gradient, bended together in a natural looking way, along with large storms which blend together an area around themselves.
 
 ## Earth-like
 Earth-like planets are similar to an Earth-like environment, these planets will have distinct continents and ocean regions, as well as biome customization such as icy for the poles, dessert for the equator, or even grassy mountainous areas in between. The mountains modify the mesh to create physical differences in the high and low areas of the planet.
 
 ## Rocky
 The rocky planet class, this class is similar to the Moon, a planet like Mars, or asteroids. This class is defined by their rocky colour palettes, whereas grey shades would be a moon or asteroid, more coloured bodies could be akin to Mars. 
 
 ## Cracked
 The Cracked class, although heavily derived from science fiction, it creates a very unique atmosphere and would be a great playable environment for a game or simulation. Its defining features are deep cracks along the planet’s surface exposing the molten interior of the planet.

 ### How to Run
 Running and switching between planets can be a bit tricky. There was not enough time to implement a proper planet switch, so a spawner class and inspector parameters are used to facilitate switching between the classes. The spawner class can be accessed under the hierarchy by selection the “Spawner”, as shown in A-1. This brings up the parameters as shown in A-2 under the inspector. The Planet Type is a dropdown which can be selected to show all of the class types, physically selecting the class will display the class on screen. Once the planet is brought forward, it must be selected again from the hierarchy on the left side of the screen.
