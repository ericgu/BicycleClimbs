<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <xsl:apply-templates select="location"/>
  </xsl:template>

  <xsl:template match="location">
    <div style="padding-right: 8px; margin-top: 2px">
      <xsl:apply-templates select="info"/>
    </div>
  </xsl:template>

  <xsl:template match="info">
  
    <xsl:variable name="page" select="../@arg0"/>
    <xsl:variable name="address">
        <xsl:for-each select="address/line">
          <xsl:if test="position() &gt; 1">, </xsl:if>
          <xsl:value-of select="."/>
        </xsl:for-each>
    </xsl:variable>

      <h3>
          <a target="_ClimbWindow">
              <xsl:attribute name="href">
                  <xsl:value-of select="url"/>
              </xsl:attribute>
              <xsl:value-of select="title"/>
          </a>
      </h3>

    <div>
	<table border="1">
	  <tr>
		<td>Length</td><td align="right"><xsl:value-of select="length"/> miles</td>
	  </tr>
	  <tr>
		<td>Elevation Gain</td><td align="right"><xsl:value-of select="elevationgain"/> feet</td>
	  </tr>
	  <tr>
		<td>Gradient</td><td align="right"><xsl:value-of select="gradient"/> %</td>
	  </tr>
	  <tr>
		<td>Max Gradient</td><td align="right"><xsl:value-of select="maxgradient"/> %</td>
	  </tr>
	</table>
    </div>
 
    <div style="font-size: small; margin-top: 14px">
      <xsl:apply-templates select="address/line"/>
    </div>

  </xsl:template>

  <xsl:template match="line">
    <div style="margin-top: 2px"><xsl:value-of select="."/></div>
  </xsl:template>

  <xsl:template name="getSingleLineAddress">
    <xsl:for-each select="address/line">
      <xsl:if test="position() &gt; 1">, </xsl:if>
      <xsl:value-of select="."/>
    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>