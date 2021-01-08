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
- [Bronvermelding](#6-bronvermelding)

## 2 Inleiding

Het algemeen idee van VerstAPpertje is een Virtual Reality Ervaring te creëren waarin een speler verstoppertje kan spelen in een 3D-wereld die gebaseerd is op de gebouwen van AP.
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

Wanneer het spel start, zal de speler op een willekeurig `spelersspawnplatform` (groen) worden gespawnd. Tegelijkertijd zal ook de zoeker op een daarvoor bestemd platform (rood) worden gespawnd. De speler heeft dan de mogelijkheid om rond te lopen in het klaslokaal en zich zo goed mogelijk te verstoppen. De zoeker zal trachten de speler te vinden. De zoeker is zoals eerder vermeld een agent die op voorhand wordt getraind.

Wanneer de speler gevonden en gepakt wordt door de zoeker, zal de zoeker deze verplaatsen richting de gevangenis. Eens deze aan de gevangenis gearriveerd is, wordt de speler hierin opgesloten. Dit is dan ook het einde van het spel. Het doel van de speler is om zo lang mogelijk uit de handen van de zoeker te blijven.

### 3.4 Observaties, mogelijke acties en beloningen

In dit project maken wij gebruik van reinforcement learning om de ML Agents op een correcte wijze te laten leren. Dit doen wij door gebruik te maken van zowel intrinsieke- als extrinsieke beloningen. Extrinsieke beloningen zijn beloningen die door ons worden gedefinieerd. Intrinsieke beloningen bepalen dan weer de nieuwsgierigheid van de ML Agents en hoe snel hij iets moet leren.

Doordat de zoeker en de Speler gemeenschappelijke gedragingen hebben (zie hoofdstuk `'Gedragingen van de objecten'`), worden deze in een superklasse gebruikt waar beiden van zullen overerven. Zo zullen ze beiden gelijkaardige gedragen tonen, maar telkens gestraft of beloond op verschillende acties.

Aangezien de zoeker het belangrijkst object is van dit project, zal hij ook meer worden beloond en afgestraft voor de acties die het zal ondernemen. De beloningen en afstraffingen focussen zich op het vangen van spelers en het forceren van een constante zoektocht. Deze worden hieronder beschreven.

| Omschrijving | Beloning (floats) |
|-|-|
| Stilstaan & niet roteren | -0,001 |
| Vangen van een speler terwijl hij er op dat ogenblik één in handen heeft | - 0,1 |
| Vangen van een speler terwijl hij er op dat ogenblik geen in handen heeft | +0,5 |
| Steken van de gevangen speler in de gevangenis | +1 |

De Speler daarentegen zal logischerwijs afgestraft worden als het door een zoeker wordt gevangen.

| Omschrijving | Beloning (floats) |
|-|-|
| Gevangen door een zoeker | -1 |

### 3.5 Beschrijvingen van de objecten

Vooraleer we aan de effectieve ML Agents training kunnen starten, zullen er eerst objecten aangemaakt moeten worden die als basis van dit project zullen dienen, beginnende met het klaslokaal.

#### Classroom object

![Classroom](https://i.imgur.com/9rknYTq.png)

Bovenstaande afbeelding geeft ons een top-down view van het volledige speelveld (niet-trainingsomgeving), in ons geval het klaslokaal. Zoals u kan zien, dient dit ook als het parent object voor alle objecten om in één enkel omgeving te plaatsen. Deze bevat het nodige gedrag om de spelomgeving foutloos te laten verlopen door tijdens de trainingen telkens het spel te herstarten als alle spelers in de gevangenis zitten. Ook zien we een aantal belangrijke elementen voor zowel de speler als de intelligente agent die als zoeker zal fungeren.

Zo'n klaslokaal zal, zoals eerder vermeld, als de container dienen waarin alle objecten terecht komen en bestaat zelf uit een een SurroundingWalls parent-object die alle rode buitenste muren (Cube-object) bevat, een Floor (pane-) object die als vloer dient en een Ceiling (pane-) object die als dak zal dienen. Voor een betere zichtbaarheid kan het dak worden disabled tijdens de trainingen.

Over het hele klaslokaal zien we dat er een aantal deuren zijn verspreid. De speler kan van deze deuren handig gebruik maken om zich beter te verstoppen voor de zoeker. De zoeker zal dan de deur moeten openen om de speler te kunnen zien. Om ervoor te zorgen dat de speler hier niet té veel voordeel uit kan halen, is er bij elke kamer die een deur bevat slechts één deur voorzien zodat de speler niet gewoon kan wachten tot de deur opengaat en dan de andere uitweg nemen. Ook zijn er heel wat muren opgezet waarachter de spelers zich kunnen verstoppen.

Door meerdere klaslokalen in een scène te zetten, kan men meerdere spelomgevingen tegelijk laten draaien. Logischerwijs zal dit ervoor zorgen dat het leerproces van de ML Agents sneller zal verlopen.

> Opgelet: deze verhoogde leercurve is er enkel zolang het toestel waar je de training op draait krachtig genoeg zijn om al deze omgevingen tegelijkertijd te kunnen draaien. Als hier geen rekening mee gehouden wordt, zorgt dit juist voor een vertraging van het leerproces.

Bij het klaslokaal is het ook belangrijk om het klaslokaal gedrag mee te geven. Hiervoor moet men aangeven hoeveel spelers en zoekers de gebruiker wenst te spawnen tijdens de trainingen of tijdens het spelverloop en de prefabs van de objecten die worden gegenereerd. In dit geval zijn dit de de speler en de zoeker prefabs. Ook wordt er een TextMeshPro-object gevraagd die de som van de rewards van alle zoekers samen (indien er meer dan een zoeker is) zal tonen.

Om trainingen effectief sneller te laten verlopen, is het beter om trainingen door te laten gaan in een kleiner klaslokaal met minder muren om achter te kunnen verstoppen. Dit verhoogt de kans dat een zoeker tegen een speler kan botsen en naar de gevangenis kan brengen. 

![Voorbeeld van een trainingsomgeving](https://cdn.discordapp.com/attachments/497393423498608662/797081291979489280/Screenshot_131.png)

Zo is het beter om bij de bovenstaande afbeelding om met het klaslokaal linksbovenaan te starten met de trainingen. Vanaf het moment dat men merkt dat de intelligente agent voldoende weet, kunnen we overschakelen naar een groter omgeving met meer muren met het verkregen brein als basis. (`mlagents-learn file.yml --initialize-from naam`)

#### Deur object

![Deur](https://i.imgur.com/9x1VfSv.png)

Bepaalde lokalen zijn enkel toegankelijk via een deur. Deze kunnen op twee manieren worden geopend. De eerste manier maakt gebruik van grabbables aan de hendels. De speler kan deze hendels vastnemen en zo de deur opentrekken of openduwen. De tweede manier is om ertegenaan te lopen. Hierbij zal de deur op een realistische manier worden opengeduwd.

De deur maakt gebruik van fixed joints om te draaien. Dit is een component gemaakt om objecten rond een specifieke as te laten draaien. Er kan een limiet gezet worden die de draaihoek van de deur beperkt. De "Door"-tag zal ook aan dit prefab worden meegegeven.

![Zichtbare hendelobject](https://i.imgur.com/MshOD0w.png)

Uiteindelijk bestaat de deur uit vier, onzichtbare, rechtoekige hendelobjecten. Twee aan elke kant van de deur. 

Het eerste hendelobject zorgt er voor dat de hendel op de plaats van de visuele deurklink blijft. 

Het tweede hendelobject is een grabable die de speler kan vastnemenen. Wanneer de speler de hendel loslaat, wordt de locatie van deze grabable terug gereset naar de locatie van het eerste hendelobject.[1]


#### Gevangenis object

![Gevangenis](https://i.imgur.com/WWgn2e4.png)

Wanneer een speler gevangen wordt door de zoeker, wordt deze in de gevangenis opgesloten. Dit gebeurt simpelweg door de collider van de speler tegen de collider van de gevangenis aan te tikken. Om de beste resultaten te halen, is het beter om de collider van de jail als een trigger in te stellen zodat botsingen daartegen niet voor ongewest gedrag zorgen.

De gevangenis bestaat uit een vloer (pane-) object, ijzeren tralies (cube-) objecten, vier muur (cube-) objecten die de traliën met elkaar verbinden en een dienblad. Ook wordt dit omringd door een nog groter kubus die uit vier cube-objecten bestaat. Dit zal nodig zijn om het glitchen van een speler in de gevangenis te voorkomen.

![Tralie met grote collider](https://i.imgur.com/lR4JUVg.png)

Ook wordt er aangeraden om de collider van de middelste tralie langs beide kanten volledig uit te strekken zodat deze even groot is als de collider van de hele jail. Dit zorgt ervoor dat de zoeker niet in de gevangenis kan kijken en niet achter spelers zou aanzitten die daar al in zitten.

![Jailtag](https://i.imgur.com/XiPzCll.png)

Aan dit object wordt de "Jail"-tag gegeven. 

#### Speler object

<img src="DocAssets/speler.png" placeholder="Speler" width="200" >

De speler is in staat om zichzelf naar voor, achter, links en rechts te verplaatsen. Ook kan deze rond de X-as (links en rechts) roteren. Zoals hierboven vermeld is er ook een interactie tussen de speler en de deuren. Deze kunnen geopend en gesloten worden. Als laatst is er nog de interactie met de gevangenis. Wanneer de zoeker de gevangenis aanraakt, zal het spel eindigen.

De Ray Perception Sensors van beide ogen van de speler zijn als volgt ingesteld:

| Variabele             | Waarde         |
| --------------------- | -------------- |
| Detectable Tags       | Wall, HideWall, **Seeker**, Door, Jail |
| Rays Per Direction    | 3              |
| Max Ray Degrees       | 4.3              |
| Sphere Cast Radius    | 0.7            |
| Ray Length            | 370             |
| Ray Layer Mask        | Mixed          |
| Stacked Raycasts      | 1              |
| Start Vertical Offset | 0              |
| End Vertical Offset   | -8            |
| Use Child Sensors     | True           |

Als volgende stap moet hier zeker het Decision Requester script op staan met "Take Actions Between Decisions" uitgevinkt.

![Decision Requester](https://i.imgur.com/mcNk5kO.png)

Om de zes mogelijke voor de speler ook effectief mogelijk te maken, zal die de `Behavior Parameters` component moeten hebben met een branch size op zes.

> ! Vergeet zeker niet om "Use Child Sensors" aan te vinken!

![Behavior parameters speler](https://i.imgur.com/ghIDOda.png)

Aan deze prefab wordt de "Player"-tag gegeven.

#### Zoeker object

<img src="https://i.imgur.com/Y3ucgt3.png" placeholder="Speler" width="200" >

| Variabele             | Waarde         |
| --------------------- | -------------- |
| Detectable Tags       | Wall, HideWall, **Seeker**, Door, Jail |
| Rays Per Direction    | 3              |
| Max Ray Degrees       | 4.3              |
| Sphere Cast Radius    | 0.7            |
| Ray Length            | 370             |
| Ray Layer Mask        | Mixed          |
| Stacked Raycasts      | 1              |
| Start Vertical Offset | 0              |
| End Vertical Offset   | -8            |
| Use Child Sensors     | True           |

Aan deze prefab wordt de "Zoeker"-tag gegeven.

#### Spawnlocation objecten

![SpawnLocations](https://i.imgur.com/5mLZZI5.png)

Om spelers en zoekers in het klaslokaal te laten spawnen en dit zo flexibel mogelijk te maken zodat deze te allen tijde veranderd kan worden, zijn er spawnlocation (cube-) objecten die de locatie markeren waar de spelers en zoekers kunnen spawnen.

![PlayerSpawnLocation](https://i.imgur.com/TPpMhFP.png)

Dit zijn, zoals eerder vermeld, simpele cube-objecten met de collider ingesteld als een trigger. Zo kunnen we de spelers en zoekers exact op dezelfde locatie als het object laten spawnen, zonder deze met elkaar te laten botsen.

Een spelerspawnlocatieobject zal de tag `PlayerSpawnLocation` moeten krijgt en de zoeker de tag `SeekerSpawnLocation`. Deze objecten moeten ook in de overeenkomstige parent-objecten (`PlayerSpawnLocations` en `SeekerSpawnLocations`) zitten.

Bij de start van elke nieuwe episode zal er over deze spawnlocaties geloopt worden om zoekers en spelers te spawnen op één van deze plaatsen.
> Er kan maar 1 zoeker/speler per spawnlocatie spawnen. Meer uitleg hierover in hoofdstuk 3.5.

### 3.5 Gedragingen van de objecten

#### Zoeker

<img src="DocAssets/zoeker.png" placeholder="Zoeker" width="200" >

De zoeker is, net zoals de speler, in staat om zichzelf naar voor, achter, links en rechts te verplaatsen en deze kan ook rond de X-as roteren. Ook heeft de zoeker de mogelijkheid om deuren te openen en te sluiten.

De zoeker heeft echter twee ogen met 3D Ray Perception Sensors. Deze zijn in staat om alle objecten met een tag te observeren. Wanneer de Ray Perception Sensors de speler zien, zou de zoeker (in theorie) zich richting de speler moeten verplaatsen, deze "vastnemen", en deze naar de gevangenis brengen. Het vastnemen van de speler doet de zoeker door simpelweg tegen de speler aan te lopen.

De Ray Perception Sensors van beide ogen van de zoeker zijn als volgt ingesteld:

| Variabele             | Waarde         |
| --------------------- | -------------- |
| Detectable Tags       | Wall, HideWall, **Player**, Door, Jail |
| Rays Per Direction    | 3              |
| Max Ray Degrees       | 4.3              |
| Sphere Cast Radius    | 0.7            |
| Ray Length            | 370             |
| Ray Layer Mask        | Mixed          |
| Stacked Raycasts      | 1              |
| Start Vertical Offset | 0              |
| End Vertical Offset   | -8            |
| Use Child Sensors     | True           |

![Decision Requester](https://i.imgur.com/mcNk5kO.png)

Als volgende stap moet hier ook het Decision Requester script op staan met "Take Actions Between Decisions" uitgevinkt.

Hoewel de speler in het uiteindelijke spel door een persoon worden gespeeld, zal deze in de trainingsfase ook worden aangedreven door een intelligente agent. Beide agents worden dus als het ware tegen elkaar opgezet en moeten beiden zo goed mogelijk hun eigen taak uitvoeren. De agent van de speler moet uit de handen van de zoeker proberen te blijven, terwijl de zoeker de speler moet vangen en deze opsluiten in de gevangenis.

Het beloningssysteem achter de zoeker en de speler wordt aangedreven door code. Aangezien beiden redelijk gelijkaardig zijn in wat ze kunnen doen, erven ze alletwee over van dezelfde superklasse: MovingObject.

```csharp
public abstract class MovingObject : Agent
    {
        [Header("Settings")]
        public float movementSpeed = 15f;
        public float rotationSpeed = 75f;

        public Classroom Classroom { get; set; }
        protected Rigidbody rbody;
        protected GameObject jailFloor;

        public override void Initialize()
        {
            Classroom = GetComponentInParent<Classroom>();
            rbody = GetComponent<Rigidbody>();
            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
            rbody.angularDrag = 50;
        }

        protected virtual void FixedUpdate()
        {
            RequestDecision();
        }

        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = 0f;
            actionsOut[1] = 0f;
            actionsOut[2] = 0f;
            actionsOut[3] = 0f;
            actionsOut[4] = 0f;
            actionsOut[5] = 0f;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                actionsOut[0] = 2f;
            }
             if (Input.GetKey(KeyCode.DownArrow))
            {
                actionsOut[1] = 1f;
            }
             if (Input.GetKey(KeyCode.LeftArrow))
            {
                actionsOut[2] = 1f;
            }
             if (Input.GetKey(KeyCode.RightArrow))
            {
                actionsOut[3] = 1f;
            }
             if (Input.GetKey(KeyCode.D))
            {
                actionsOut[4] = 1f;
            }
             if (Input.GetKey(KeyCode.A))
            {
                actionsOut[5] = 1f;
            }
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            if (vectorAction[0] > 0.5f)
            {
                Vector3 rightVelocity = new Vector3(movementSpeed * vectorAction[0], 0f, 0f);
                rbody.velocity = rightVelocity;
            }
            if (vectorAction[1] > 0.5f)
            {
                Vector3 leftVelocity = new Vector3(-movementSpeed * vectorAction[1], 0f, 0f);
                rbody.velocity = leftVelocity;
            }
            if (vectorAction[2] > 0.5f)
            {
                Vector3 rightVelocity = new Vector3(0f, 0f, movementSpeed * vectorAction[2]);
                rbody.velocity = rightVelocity;
            }
            if (vectorAction[3] > 0.5f)
            {
                Vector3 leftVelocity = new Vector3(0f, 0f, -movementSpeed * vectorAction[3]);
                rbody.velocity = leftVelocity;
            }

            if (vectorAction[4] > 0f)
            {
                transform.Rotate(0f, (vectorAction[4] * rotationSpeed) * Time.deltaTime, 0f);
            }
            else if (vectorAction[5] > 0f)
            {
                transform.Rotate(0f, (vectorAction[5] * rotationSpeed) * Time.deltaTime * -1, 0f);
            }
        }
    }
```

Het script dat de zoeker aandrijft:

```csharp
public class Seeker : MovingObject
    {
        public Player CapturedPlayer { get; set; }
        public bool HasPlayerGrabbed { get; set; }
        public int PlayerCount { get; set; }
        public int PlayersCaptured { get; set; }

        public override void CollectObservations(VectorSensor sensor)
        {
            base.CollectObservations(sensor);

            sensor.AddObservation(HasPlayerGrabbed);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (CapturedPlayer != null && CapturedPlayer.IsGrabbed && !CapturedPlayer.IsJailed)
            {
                TransportPlayer();
            }

        }

        private void TransportPlayer()
        {
            if (CapturedPlayer != null)
            {
                CapturedPlayer.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            }
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            base.OnActionReceived(vectorAction);

            if (vectorAction[0] == 0f && vectorAction[1] == 0f && vectorAction[2] == 0f && vectorAction[3] == 0f && vectorAction[4] == 0f && vectorAction[5] == 0f)
            {
                AddReward(-0.001f);
            }
        }

        public override void OnEpisodeBegin()
        {
            Classroom = GetComponentInParent<Classroom>();

            if (Classroom != null)
            {
                Classroom.ClearEnvironment();
                Classroom.ResetSpawnSettings();
                Classroom.SpawnPlayers();
                Classroom.SpawnSeekers();
                PlayerCount = Classroom.playerCount;
            }

            PlayersCaptured = 0;
            HasPlayerGrabbed = false;
            CapturedPlayer = null;
        }

        protected void OnCollisionEnter(Collision collision)
        {
            Transform collObject = collision.transform;

            if (collObject.CompareTag("Player"))
            {
                if (!HasPlayerGrabbed)
                {
                    HasPlayerGrabbed = true;

                    CapturedPlayer = collObject.gameObject.GetComponent<Player>();
                    if (CapturedPlayer != null && !CapturedPlayer.IsJailed)
                    {
                        CapturedPlayer.IsGrabbed = true;
                        CapturedPlayer.CapturedBy = this;
                        CapturedPlayer.AddReward(-1f);
                        AddReward(0.5f);
                    }
                }
                else
                {
                    AddReward(-0.1f);
                }
            }
            else if (collObject.CompareTag("JailFloor"))
            {
                EndEpisode();
            }
        }

        public void EndEpisodeLogic()
        {
            if (PlayersCaptured == PlayerCount)
            {
                EndEpisode();
            }
        }

        public void ClearCapturedPlayer()
        {
            PlayersCaptured++;
            AddReward(1f);
            HasPlayerGrabbed = false;
            CapturedPlayer = null;
            EndEpisodeLogic();
        }
    }
```

Het script dat de speler aandrijft:

```csharp
    public class Player : MovingObject
    {
        public bool IsJailed { get; set; }
        public bool IsGrabbed { get; set; }
        public Seeker CapturedBy { get; set; }
        public override void CollectObservations(VectorSensor sensor)
        {
            base.CollectObservations(sensor);

            sensor.AddObservation(IsJailed);
            sensor.AddObservation(IsGrabbed);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            if (!IsGrabbed)
            {
                base.OnActionReceived(vectorAction);
            }
        }

        public void CapturedLogic()
        {
            IsJailed = true;
            IsGrabbed = false;
            CapturedBy.HasPlayerGrabbed = false;
            CapturedBy = null;
        }
    }
```

Om ervoor te zorgen dat de Speler en de zoeker nu ook effectief kunnen worden aangedreven door ML-agents, moeten volgende componenten worden ingesteld voor de Speler in de inspector in Unity:

![Speler Inspector](DocAssets/spelercomponenten.png)

Daarnaast moet ongeveer hetzelfde gebeuren voor de zoeker:

![Zoeker Inspector](DocAssets/zoekercomponenten.png)

Zorg ervoor dat alle instellingen zeker exact hetzelfde staan ingesteld. Anders is het zeer waarschijnlijk dat het trainen zal mislukken.

Het configuratiebestand om beide agents te trainen is het volgend yml-bestand. Hierbij hebben we  met de _curiosity strength_ parameter gespeeld tot dat we aan de optimale waarden kwamen voor de training. We merkten dat het belangrijk is om hogere curiosity waarden toe te kennen wanneer er met complexe omgevingen getraind wordt. Dit zorgt ervoor dat de agent de omgeving beter gaat verkennen.

```yml
behaviors:
  Seeker:
    trainer_type: ppo
    max_steps: 5.0e7
    time_horizon: 64
    summary_freq: 10000
    keep_checkpoints: 5
    checkpoint_interval: 50000
    
    hyperparameters:
      batch_size: 32
      buffer_size: 9600
      learning_rate: 3.0e-4
      learning_rate_schedule: constant
      beta: 5.0e-3
      epsilon: 0.2
      lambd: 0.95
      num_epoch: 3

    network_settings:
      num_layers: 2
      hidden_units: 128
      normalize: false
      vis_encoder_type: simple

    reward_signals:
      extrinsic:
        strength: 1.0
        gamma: 0.99
      curiosity:
        strength: 0.1
        gamma: 0.99
        encoding_size: 256
        learning_rate : 1e-3
  Player:
      trainer_type: ppo
      max_steps: 5.0e7
      time_horizon: 64
      summary_freq: 10000
      keep_checkpoints: 5
      checkpoint_interval: 50000
      
      hyperparameters:
        batch_size: 32
        buffer_size: 9600
        learning_rate: 3.0e-4
        learning_rate_schedule: constant
        beta: 5.0e-3
        epsilon: 0.2
        lambd: 0.95
        num_epoch: 3

      network_settings:
        num_layers: 2
        hidden_units: 128
        normalize: false
        vis_encoder_type: simple

      reward_signals:
        extrinsic:
          strength: 1.0
          gamma: 0.99
        curiosity:
          strength: 0.1
          gamma: 0.99
          encoding_size: 256
          learning_rate : 1e-3
```

#### Deur

Om de deur vlekkeloos te laten werken, wordt er gebruik gemaakt van deze scripts.
In de update functie wordt er bekeken of het grabable handleobject niet te ver is van de deurklink zelf. Indien dit wel is, zal de deur losgelaten worden. `GrabEnd()` wordt aangeroepen wanneer de speler het object loslaat. 

```csharp
public class DoorGrabbable : OVRGrabbable
{
    public Transform handler;

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(Vector3.zero, Vector3.zero);

        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;

        Rigidbody rbhandler = handler.GetComponent<Rigidbody>();
        rbhandler.velocity = Vector3.zero;
        rbhandler.angularVelocity = Vector3.zero;
    }

    public void Update()
    {
        if (Vector3.Distance(handler.position, transform.position) > 0.4f)
        {
            grabbedBy.ForceRelease(this);
        }
    }
}
```


### Classroom

Het classroom object is verantwoordelijk voor het spawnen van de speler-, en zoekerobjecten. De zoeker en speler worden bij elke episode op een willekekeurig spawnplatform gespawned. Dit wordt gedaan door gebruik te maken van de methodes `GetAvailableSpawnLocation()`, `GetRandomSpawnLocation()`, `SpawnSeekers()`, `SpawnPlayers()`

```csharp
public Vector3 GetAvailableSpawnLocation(MovingObjectTypes type)
        {
            SpawnLocation spawnLocation = GetRandomSpawnLocation(type);
            spawnLocation.IsUsed = true;
            Vector3 pos = spawnLocation.transform.position;
            return new Vector3(pos.x, pos.y, pos.z);
        }

        private SpawnLocation GetRandomSpawnLocation(MovingObjectTypes type)
        {
            IEnumerable<SpawnLocation> locations;
            switch (type)
            {
                case MovingObjectTypes.SEEKER:
                    locations = seekerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>();
                    break;
                case MovingObjectTypes.PLAYER:
                    locations = playerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>();
                    break;
                default:
                    locations = null;
                    break;
            }
            locations = locations.Where(x => !x.IsUsed);
            int randomIndex = Random.Range(0, locations.Count());
            SpawnLocation randomlyPicked = locations.ElementAt(randomIndex);
            return randomlyPicked;
        }

        public void SpawnSeekers()
        {
            seekers.transform.SetParent(transform);
            // Moet het aantal gevraagde seekers spawnen, maar ook rekening houden met hoeveel spawnplaatsen er effectief zijn.
            for (int i = 0; i < seekerCount && i < seekerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>().Length; i++)
            {
                GameObject seeker = Instantiate(seekerPrefab.gameObject);

                seeker.transform.localPosition = GetAvailableSpawnLocation(MovingObjectTypes.SEEKER);
                seeker.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                var component = seeker.GetComponent<Seeker>();
                component.PlayerCount = playerCount;
                component.HasPlayerGrabbed = false;

                seeker.transform.SetParent(seekers.transform);
            }
        }

        public void SpawnPlayers()
        {
            players.transform.SetParent(transform);

            // Moet het aantal gevraagde seekers spawnen, maar ook rekening houden met hoeveel spawnplaatsen er effectief zijn.
            for (int i = 0; i < playerCount && i < playerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>().Length; i++)
            {
                GameObject seeker = Instantiate(playerPrefab.gameObject);

                seeker.transform.localPosition = GetAvailableSpawnLocation(MovingObjectTypes.PLAYER);
                seeker.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                var component = seeker.GetComponent<Player>();
                component.IsGrabbed = false;
                component.IsJailed = false;

                seeker.transform.SetParent(players.transform);
            }
        }
```

De `ClearEnvironment()` methode zorgt er voor dat het speelveld leeg is vooraleer er een nieuwe episode begint.

```csharp
    public void ClearEnvironment()
        {
            foreach (Transform player in players.transform)
            {
                Destroy(player.gameObject);
            }
            foreach (Transform seeker in seekers.transform)
            {
                Destroy(seeker.gameObject);
            }
        }
```

`ResetSpawnSettings()` zorgt ervoor dat alle spawnlocaties weer vrij worden gemaakt voor de volgende keer dat spelers of zoekers moeten worden gespawnd.

```csharp
        public void ResetSpawnSettings()
        {
            foreach (SpawnLocation sl in playerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>())
            {
                sl.IsUsed = false;
            }

            foreach (SpawnLocation sl in seekerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>())
            {
                sl.IsUsed = false;
            }
        }
```

`GetAvailableSpawnLocation()` 
```csharp
        public Vector3 GetAvailableSpawnLocation(MovingObjectTypes type)
        {
            SpawnLocation spawnLocation = GetRandomSpawnLocation(type);
            spawnLocation.IsUsed = true;
            Vector3 pos = spawnLocation.transform.position;
            return new Vector3(pos.x, pos.y, pos.z);
        }
```

### Spawnlocations
Wanneer de property _IsUsed_ op _true_ komt te staan zal deze niet meer gebruikt kunnen worden om de agents te spawnen.
```csharp
    public class SpawnLocation : MonoBehaviour
    {
        public bool IsUsed { get; set; } = false;
    }
```


### 3.6 One-Pager

#### 3.6.1 Inleiding

Het algemeen idee is om een Virtual Reality Ervaring te maken waarin de gebruiker verstoppertje kan spelen in een 3D-wereld gebaseerd op de gebouwen van AP. De speler zelf zal zich altijd moeten verstoppen, terwijl een intelligente agent hem zal trachten te vinden. 

#### 3.6.2 AI Component

Zonder de AI-component zal het onmogelijk zijn voor de zoeker om de verstopper snel te vinden. Hiervoor zal 
de agent gebruik maken van de aanwijzingen. Ook is deze belangrijk om van ons spel een soloplayer avontuur 
te kunnen maken. Anders zal elke verstopper afhankelijk zijn van een tweede partij, nl. de zoeker.  
Met een AI-Component zal de “zoeker” met behulp van Ray Perception Sensors studenten kunnen zien, welke 
deuren er openstaan, welke stoelen er verplaatst zijn, etc. 
Wij opteren hierbij voor een Single-Agent aangezien er slechts één zoeker zal zijn. 

#### 3.6.3 Interacties

De “zoeker” van het spel zal gespeeld worden door een Intelligence Agent. Zoals een gewone speler zal deze 
getraind worden om bepaalde geluiden en visuele aanwijzingen te gebruiken om de “verstopper” te vinden. 
De agent zal dankzij het Ray Perception 3D component de mogelijkheid hebben om andere gameobjects met 
op voorhand ingestelde tags te zien. 
De enige virtueel fysieke interactie tussen de agent en de speler zal zijn wanneer de speler gevonden wordt. De 
speler wordt dan meegenomen naar de gevangenis door de agent, waar hij opgesloten zal worden. 

## 4 Resultaten

### 4.1 TensorBoard

![Resultaten](DocAssets/cumulativerewards.png)

In de bovenstaande grafiek zien we dat er veel lijnen haast symmetrisch ten op zichte van de middellijn lopen. Dit komt natuurlijk door het feit dat de 2 agents concurrenten van elkaar zijn. Als de zoeker punten verliest, wint de speler punten en vice versa. Na verloop van tijd worden zowel de zoeker als de Speler beter in hun taak en gaan hun scores logischerwijs veel dichter bij elkaar liggen.

### 4.2 Opvallende waarnemingen

Om de zoeker aan te leren dat hij naar een speler moest zoeken, moest er ook een speler Agent aangemaakt worden die zich zou kunnen verstoppen. Op een gegeven moment was de Agent van de speler te slim geworden voor de Agent van de zoeker. Dit zorgde ervoor dat de snelle vooruitgang van de Seeker werd belemmerd.

Zowel de speler als de zoeker had een manier gevonden om in de gevangenis te geraken zonder het beoogde spelverloop hierbij te volgen. De speler kon op een onvoorspelde manier de gevangenis in. Dit zorgde ervoor dat hij veilig was van de zoeker. De speler daarentegen ging rechtstreeks richting de gevangenis. Hierdoor kon hij de episode eindigen alvorens hij afgestraft werd voor strafbaar gedrag. Op deze manier waren zijn scores hoger dan dat hij zou zoeken en zo punten zou verliezen.

Elke keer dat er gedacht werd dat alle bugs uit de applicatie waren, vonden de speler en zoeker toch nog een manier om een bug te abusen. Dit maakte het extra moeilijk om te trainen.


## 5 Concusie

Wij als groep hebben een VR applicatie gemaakt voor een enkele speler die een soort "verstoppertje" nabootst, genaamd verstAPpertje.

Het grootste probleem van deze opdracht was de gelimiteerde tijdsspanne. Dit zorgde ervoor dat de Agent niet de kans had om volledig te ontwikkelen. Enkele voorgestelde verbeteringen hiervoor zijn: het beloningssysteem nog verder optimaliseren, de agent nog meer tijd geven om bij te leren of een supercomputer gebruiken zodat de berekeningen sneller gaan.

## 6 Bronvermelding

VR with Andrew (Mar 18, 2020): Moving in VR using Unity's XR Toolkit [1] and Moving in VR using Unity's XR Toolkit [2] :
1. https://www.youtube.com/watch?v=6N__0jeg6k0 

2. https://www.youtube.com/watch?v=X2QYjhu4_G4  

How to make a door in VR - Unity tutorial (Aug 21, 2019) by Valem opgehaald van https://www.youtube.com/watch?v=3cJ_uq1m-dg [3]

D'haese, D. "Gedragingen van de agent en de andere spelobjecten" [4] https://ddhaese.github.io/Course_ML-Agents/gedragingen-van-de-agent-en-de-andere-spelobjecten.html#obelix.cs