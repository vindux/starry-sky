# Starry Sky ✨

## Features

Das Projekt umfasst die Erstellung einer Sternenhimmelanimation durch einen Projektor, wobei bewegliche Partikel als Sterne fungieren. Diese Partikel zeigen minimale Eigenbewegungen und folgen den Bewegungen im Raum durch Personentracking. Jeder Mensch wird durch einen kleinen Sternencluster repräsentiert.

### Personeninteraktion
- Bei zwei Personen verbinden sich die Cluster zu einem größeren Cluster.
- Bei drei Personen löst sich ein Supernova-Cluster aus.
- Die Bewegungen der Sterne intensivieren sich mit der Größe des Clusters.

### Audiovisuelles Erlebnis
- Passend zum visuellen Geschehen wird ein atmosphärischer Sound abgespielt.
- Unterschiedliche Audio-Feedbacks begleiten die Größe der Cluster.

## Aufbau

### Raumvoraussetzungen
- Dunkler Raum für optimale Projektion.

### Hardware
- FUJIFILM FP Z5000 Projektor für großflächige Deckenprojektion.
- Microsoft Kinect 2 für markerloses Personentracking.

### Aufstellung
- Beamer in einer Raumecke für maximale Deckenabdeckung.
- Kinect an der längeren Raumseite für umfassende Personenverfolgung.

### Software
- Unity-Anwendung zur Verarbeitung der Tiefendaten, Sternenhimmelrendering und Projektorbildgenerierung.

### Kern der Anwendung
- Partikelsystem in Unity repräsentiert den Sternenhimmel.
- Forcefields beeinflussen das System, indem sie größere Sternenansammlungen für erkannte Personen erzeugen.
- C#-Skript ermöglicht die Bildung von Clustern bei Annäherung von Personen.

**Hinweis:** Die Anwendung erzeugt ein immersives Erlebnis durch die Verbindung von visuellen und auditiven Elementen, wobei die Interaktion der Personen im Raum die Animation und den Sound beeinflusst.
