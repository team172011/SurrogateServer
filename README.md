# SurrogateServer

## WPF-Rahmenprogramm des Prototyps

## Aufbau
![Bild Rahmenprogramm][surrogateFramework]
### Module
![Bild Rahmenprogramm][controller]
### Verbindungen
![Bild Rahmenprogramm][connections]
### Eigenschaften
![Bild Rahmenprogramm][properties]

## Benötigte Abhängigkeiten zum Kompilieren:
 - Microsoft Visual Studio (Community 2017)
 - Datenbankmanagementsystem (z.B. [Microsoft SQL-Server](https://www.microsoft.com/de-de/download/details.aspx?id=55994))
 - [EmguCV 3.4](http://www.emgu.com/wiki/index.php/Download_And_Installation)
     - _Hinweis_: nur die EmguWorld.dll einbinden und die x64 und x86 Ordner nach Debug/Release kopieren
 - [OpenTok .Net-SDK](https://tokbox.com/developer/sdks/dot-net/)
     - _Hinweis_: Über den NuGet-Package-Manager installierbar
 - [log4net](https://logging.apache.org/log4net/download_log4net.cgi)
      - _Hinweis_: Über den NuGet-Package-Manager installierbar
 - [SharpDX Input](http://sharpdx.org/)
      - _Hinweis_: Über den NuGet-Package-Manager installierbar



[surrogateFramework]: Surrogate/resources/SurrogateFramework.jpg
[controller]: Surrogate/resources/Controller.jpg
[connections]: Surrogate/resources/Connections.jpg
[properties]: Surrogate/resources/Properties.jpg