<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureCloudService1" generation="1" functional="0" release="0" Id="39af1c82-66b2-4008-bac1-54db7b37b1ac" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureCloudService1Group" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="DaiWebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AzureCloudService1/AzureCloudService1Group/LB:DaiWebRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="DaiWebRole:con" defaultValue="">
          <maps>
            <mapMoniker name="/AzureCloudService1/AzureCloudService1Group/MapDaiWebRole:con" />
          </maps>
        </aCS>
        <aCS name="DaiWebRole:StorageConString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureCloudService1/AzureCloudService1Group/MapDaiWebRole:StorageConString" />
          </maps>
        </aCS>
        <aCS name="DaiWebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureCloudService1/AzureCloudService1Group/MapDaiWebRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:DaiWebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AzureCloudService1/AzureCloudService1Group/DaiWebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapDaiWebRole:con" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureCloudService1/AzureCloudService1Group/DaiWebRole/con" />
          </setting>
        </map>
        <map name="MapDaiWebRole:StorageConString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureCloudService1/AzureCloudService1Group/DaiWebRole/StorageConString" />
          </setting>
        </map>
        <map name="MapDaiWebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureCloudService1/AzureCloudService1Group/DaiWebRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="DaiWebRole" generation="1" functional="0" release="0" software="C:\Users\helder\documents\visual studio 2013\Projects\AzureCloudService1\AzureCloudService1\csx\Release\roles\DaiWebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="con" defaultValue="" />
              <aCS name="StorageConString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;DaiWebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;DaiWebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureCloudService1/AzureCloudService1Group/DaiWebRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AzureCloudService1/AzureCloudService1Group/DaiWebRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AzureCloudService1/AzureCloudService1Group/DaiWebRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="DaiWebRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="DaiWebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="DaiWebRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="82e3a586-c90e-4e9c-b29e-c880bfe88e4e" ref="Microsoft.RedDog.Contract\ServiceContract\AzureCloudService1Contract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="6790bd8e-4fc4-4333-8ee6-4810a888008a" ref="Microsoft.RedDog.Contract\Interface\DaiWebRole:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureCloudService1/AzureCloudService1Group/DaiWebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>