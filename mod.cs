/**
* <author>Spencer Maghrouri</author>
* <url>spencer@maghrouri.net</url>
* <credits>LifX Example Server Mod</credits>
* <description>This mod will query and kick players during Judgement Hour who are not part of any guild.</description>
* <license>GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007</license>
*/

// Register your mod as an object in the game engine, important for loading and unloading a package (mod)
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
    LiFx::registerCallback($LiFx::hooks::onTick, checkPlayers, LiFxJudgementHourFix);
  }
  
  function LiFxJudgementHourFix::checkPlayers(%this) {
    dbi.Select(LiFxJudgementHourFix,"kickClaimlessPlayers", "SELECT AccountID,Action FROM gm_action_log WHERE ActionTimeStamp BETWEEN FROM_UNIXTIME(" @ LiFxAdminOversight.LastCall @") AND CURRENT_TIMESTAMP()");
    LiFxJudgementHourFix.LastCall = getUnixTime();
  }

  function LiFxJudgementHourFix::kickClaimlessPlayers(%this, %resultSet) {
    if(%resultSet.ok()) {
      while (%resultSet.nextRecord()) {
       
      }
    }   
    dbi.remove(%resultSet);
    %resultSet.delete();
  }

};

// This command is from Torque, and activates your package so that the engine can reference it
// This is required for your mod to work, and have the code loaded in torque engine.
activatePackage(LiFxJudgementHourFix);
LiFx::registerCallback($LiFx::hooks::mods, setup, LiFxJudgementHourFix);