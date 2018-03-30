# Village agents

Village agents is an experimentation with Unity's ML-Agents. This project is a simple simulation of a village using a multi agent model.

This project is juste a multi agent exemple for now, it is not using any neural net brains (yet).

## Village
![Village exemple screenshot](Screenshots/Village01.png)

## Village agent
This main agent is the Village agent. This agent is managing the village ressources and population.
ressources:
- Food: used by villagers
- Coal: every house is using coal
- Happiness: it is the average happiness of the population used to determine if new villager will be created
- Population: The village population is limited by the number of houses in the village

Depending on the ressources and production the village emits job offers

## Villager agent
The villager agent belongs to a village and he is defined by his strength and happiness. The agent chooses between 4 actions:
- Wander: if the agents doesn't find a job offer
- Rest: if the agent strength is lower than 30 he goes to rest at home
- Fun: if the agent strength is lower than 30 he goes to the bar
- Work: if the agent is strong enought to work and there is a job offer available
