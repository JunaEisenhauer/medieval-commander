<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.2.4" name="Objects" tilewidth="1440" tileheight="1080" tilecount="4" columns="0">
 <grid orientation="orthogonal" width="1" height="1"/>
 <tile id="4" type="Fortress">
  <image width="500" height="422" source="../Fortress0.png"/>
 </tile>
 <tile id="5" type="Fortress">
  <properties>
   <property name="enemy" type="bool" value="true"/>
  </properties>
  <image width="500" height="422" source="../Fortress1.png"/>
 </tile>
 <tile id="6" type="Tower">
  <image width="1440" height="1080" source="../Tower0.png"/>
 </tile>
 <tile id="7" type="Tower">
  <properties>
   <property name="enemy" type="bool" value="true"/>
  </properties>
  <image width="1440" height="1080" source="../Tower1.png"/>
 </tile>
</tileset>
