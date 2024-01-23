/**
* <author>Spencer Maghrouri</author>
* <url>spencer@maghrouri.net</url>
* <credits>LifX Example Server Mod</credits>
* <description>This mod will query and kick players during Judgement Hour who are not part of any guild.</description>
* <license>GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007</license>
*/
if (!isObject(LiFxJudgementHourFix))
{
    new ScriptObject(LiFxJudgementHourFix)
    {
    };
}

package LiFxJudgementHourFix
{

  function LiFxJudgementHourFix::version() {
    return "v1.0.0.JHFix" 
  }
  
  function LiFxJudgementHourFix::setup() {
    LiFx::registerCallback($LiFx::hooks::onTick, checkJHStatus, LiFxJudgementHourFix);
  }
  
  function LiFxJudgementHourFix::checkPlayers(%this) {
    dbi.Select(LiFxJudgementHourFix,"kickClaimlessPlayers", "SELECT c.ID AS ClientId FROM `lifx_character` lc LEFT JOIN `character` c ON c.ID = lc.id CROSS JOIN nyu_ttmod_info info WHERE (c.GuildID IS NULL) AND (lc.loggedIn > info.boot_time) AND ((lc.loggedOut < lc.loggedIn) OR (lc.loggedOut IS null))");
  }

  function LiFxJudgementHourFix::checkJHStatus(%this) {
    if (isJHActive()) {
      checkPlayers();
    }
  }

  function LiFxJudgementHourFix::kickClaimlessPlayers(%this, %resultSet) {
    if(%resultSet.ok()) {
      while (%resultSet.nextRecord()) {

        %ClientID = %resultSet.getFieldValue("ClientID");

        for(%id = 0; %id < ClientGroup.getCount(); %id++)
        {
          %client = ClientGroup.getObject(%id);

          if(%ClientID == %client.getCharacterId())
          {
            warn("Character " SPC %client SPC " kicked for being guildless during JH");
            %client.scheduleDelete("You have been ejected from the server for having no guild during JH", 100);
            break;
          } 
        }
      }
    }   
    dbi.remove(%resultSet);
    %resultSet.delete();
  }

};

activatePackage(LiFxJudgementHourFix);
LiFx::registerCallback($LiFx::hooks::mods, setup, LiFxJudgementHourFix);