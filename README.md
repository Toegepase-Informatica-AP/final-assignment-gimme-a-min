# VerstAPpertje

## 1 Inhoud

- [Inleiding](#2-Inleiding) 
- [Methoden](#3-Methoden)
  - [Samenvatting](#3.1-Samenvatting)
  - [Installatie](#3.2-Installatie)
  - [Spelverloop](#3.3-Spelverloop)
  - [Observaties, mogelijke acties en beloningen](#3.4-Observaties,-mogelijke-acties-en-beloningen)
  - [Gedragingen van de objecten](#3.5-Gedragingen-van-de-objecten)
  - [One-Pager](#3.6-One-Pager)
- [Resultaten](#4-Resultaten)
  - [TensorBoard](#4.1-TensorBoard)
  - [Opvallende waarnemingen](#4.2-Opvallende-waarnemingen)
- [Conclusie](#5-Conclusie)

## 2 Inleiding

Het algemeen idee van VerstAPpertje is een Virtual Reality Ervaring te creëren waarin een een speler verstoppertje kan spelen in een 3D-wereld die gebaseerd is op de gebouwen van AP.
De zoeker is een intelligente agent welke op voorhand getraind is om de speler te zoeken en vervolgens deze op te sluiten in het gevang.

Hieronder een ruwe voorstelling van het beloningssysteem dat gebruikt wordt om de zoeker te trainen.

![Beloningssysteem](DocAssets/spelverloop.svg)

## 3 Methoden
### 3.1 Samenvatting

In dit document zullen alle stappen om dit project te realiseren worden toegelicht. Na het lezen hiervan zal de lezer in staat zijn om zelf een VR Ervaring ondersteund door ML te creëren met behulp van Unity, ML Agents, XR Interaction Toolkit & Oculus XR Plugin.

### 3.2 Installatie

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

### 3.3 Spelverloop

Wanneer het spel start zal de speler op een willekeurig `spelersspawnplatform` (groen) worden gespawnd. Tegelijkertijd zal ook de zoeker op een daarvoor bestemd platform (rood) worden gespawnd. De speler heeft dan de mogelijkheid om rond te lopen in het speelveld en zich zo goed mogelijk te verstoppen. De zoeker zal trachten de speler te vinden. De zoeker is zoals eerder vermeld een agent die op voorhand wordt getraind.
Wanneer de speler gevonden en gepakt wordt door de zoeker, zal de zoeker deze verplaatsen richting de gevangenis. Eens deze aan de gevangenis gearriveerd is, wordt de speler hierin opgesloten. Dit is dan ook het einde van het spel. Het doel van de speler is om zo lang mogelijk uit de handen van de zoeker te blijven.

### 3.4 Observaties, mogelijke acties en beloningen

![Speelveld](DocAssets/speelveld.png)

Bovenstaande afbeelding geeft ons een top-down view van het volledige speelveld. We zien hier een aantal belangrijke elementen voor zowel de speler als de intelligente agent die als zoeker zal fungeren.

Over het hele speelveld zien we dat er een aantal deuren zijn verspreid. De speler kan van deze deuren handig gebruik maken om zich beter te verstoppen voor de zoeker. De zoeker zal dan de deur moeten openen om de speler te kunnen zien. Om ervoor te zorgen dat de speler hier niet té veel voordeel uit kan halen, is er bij elke kamer die een deur bevat slechts één deur voorzien, zodat de speler niet gewoon kan wachten tot de deur open gaat en dan de andere uitweg nemen.

![Deur](DocAssets/deur.png)

Bepaalde lokalen zijn enkel toegankelijk via een deur. Deze kan op twee manieren geopent worden. De eerste manier maakt gebruik van grabables aan de hendels. De speler kan deze hendels vastnemen en zo de deur opentrekken of openduwen. De tweede manier is gewoon ertegenaan lopen. Hierbij zal de deur op een realistische manier opengeduwd worden.

![Gevangenis](DocAssets/gevangenis.png)

Wanneer een speler gevangen wordt door de zoeker, wordt deze in de gevangenis opgesloten. Dit gebeurt simpelweg door de collider van de speler tegen de collider van de gevangenis aan te tikken.

![Speler](DocAssets/speler.png)

De speler is in staat om zichzelf naar voor, achter, links en rechts te verplaatsen. Ook kan deze rond de X-as roteren. Zoals hierboven vermeld is er ook een interactie tussen de speler en de deuren. Deze kunnen geopend en gesloten worden. Uiteindelijk is er nog de interactie met de gevangenis. Wanneer de speler de gevangenis aanraakt, zal het spel eindigen.3D

![Zoeker](DocAssets/zoeker.png)

De zoeker is, net zoals de speler, in staat om zichzelf naar voor, achter, links en rechts te verplaatsen en deze kan ook rond de X-as roteren. Ook heeft de zoeker de mogelijkheid om deuren te openen en te sluiten.

De zoeker heeft echter twee ogen met 3D Ray Perception Sensors. Deze zijn in staat om alle objecten met een tag te observeren. Wanneer de Ray Perception Sensors de speler zien, zou de zoeker (in theorie) zich richting de speler moeten verplaatsen, deze "vastnemen", en deze naar de gevangenis brengen. Het vastnemen van de speler doet de zoeker door simpelweg tegen de speler aan te lopen. 


### 3.5 Gedragingen van de objecten



### 3.6 One-Pager



## 4 Resultaten

### 4.1 TensorBoard

### 4.2 Opvallende waarnemingen

## 5 Conclusie
