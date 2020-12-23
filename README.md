# VerstAPpertje

## Inleiding

Het algemeen idee van VerstAPpertje is een Virtual Reality Ervaring te creëren waarin een een speler verstoppertje kan spelen in een 3D-wereld die gebaseerd is op de gebouwen van AP.
De zoeker is een intelligente agent welke op voorhand getraind is om de speler te zoeken en vervolgens deze op te sluiten in het gevang.

Hieronder een ruwe voorstelling van het beloningssysteem dat gebruikt wordt om de zoeker te trainen.

![Spelverloop](DocAssets/spelverloop.svg)

## Samenvatting

In dit document zullen alle stappen om dit project te realiseren worden toegelicht. Na het lezen hiervan zal de lezer in staat zijn om zelf een VR Ervaring ondersteund door ML te creëren met behulp van Unity, ML Agents, XR Interaction Toolkit & Oculus XR Plugin.

## Installatie

Voor we kunnen starten met de ontwikkeling van het project, hebben we bepaalde software nodig.

- [Unity 2019.4.10](https://unity3d.com/unity/whats-new/2019.4.10)
  - [ML agents 1.0.5](https://docs.unity3d.com/Packages/com.unity.ml-agents@1.0/manual/index.html)
  - Oculus XR Plugin 1.4.3
  - Windows XR Plugin 2.3.0
  - XR Interaction Toolkit preview - 0.9.4
  - XR Plugin Management
  - TextMeshPro 2.1.1
- [Python 3.7.9](https://chocolatey.org/packages/python/3.7.9)
  - [ML agents 0.20.0](https://pypi.org/project/mlagents/0.20.0/)
  - [Tensorboard 2.3.0](https://pypi.org/project/tensorboard/2.3.0/)

