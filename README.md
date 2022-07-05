# gbm-crowd

## DISCLAIMER

- This is implementation is very naive and not production-ready (not even close).
- Results from this implementation are different from those seen in the papers because some details are different, such as obstacles are seen as cylinders instead of cones (like in the original papers).
- I plan to keep working on this and eventually make improvements in the future, but I can't promise anything.

## Vision-based crowd simulation

This repository contains the implementation of two approaches for steering crowds based on synthetic vision:
1. (My PhD work) T. B. Dutra, R. Marques, J.B. Cavalcante-Neto, C. A. Vidal, and J. Pettré. 2017. **Gradient-based steering for vision-based crowd simulation algorithms**. Comput. Graph. Forum 36, 2 (May 2017), 337–348. https://dl.acm.org/doi/10.5555/3128975.3129006
2. Jan Ondřej, Julien Pettré, Anne-Hélène Olivier, and Stéphane Donikian. 2010. **A synthetic-vision based steering approach for crowd simulation**. ACM Trans. Graph. 29, 4, Article 123 (July 2010), 9 pages. https://doi.org/10.1145/1778765.1778860

### Concept

In algorithms for steering agents based on synthetic vision, agents make decisions based on what they actually see in the virtual world. In other words, at every simulation step it's necessary to render what every agent sees and process the rendered images to determine the action to take before the next step.

## Dependencies

- Unity 2021.3.2f1 (I haven't tried other versions)

## Instructions

The project has only one scene called `VisionBasedCrowdSteering`. If you run it, you will see that the UI allows you to change the agent logic and the scenario. 

There are two agent implementations:
- AgentGBM, where GBM stands for Gradient-Based Model and represents the implementation of Dutra et al. [1]; and,
- AgentPRM, where PRM stands for Pure Reactive Model and represents the implementation of Ondřej et al. [2].

There are six scenarios available: Circle, Columns, Corridor, Crossing, Crossing2 and Opposite.

### Settings

All settings can be found in the folder `Settings`. 
- Agents Settings (e.g individual model parameters, comfort speed, etc.) can be found under `Agents` folder.
- Scenarios Settings (e.g. number of agents, etc.) under `Scenarios` folder.
- Simulators Settings (e.g. camera configuration, shader, etc.) under `Simulators` folder.

## License

This project is licensed under the [MIT License](LICENSE)
