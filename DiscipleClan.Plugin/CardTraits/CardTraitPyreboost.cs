using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Effect;

namespace DiscipleClan.Plugin.CardTraits
{
    /// <summary>
    /// When this card deals damage, replace with Pyre Attack × Pyre Num Attacks × number of Pyreboost traits on the card.
    /// Used by Flashfire. Ported from MT1 CardTraitPyreboost.
    /// </summary>
    public class CardTraitPyreboost : CardTraitState
    {
        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions();
        }
        public override int OnApplyingDamage(ApplyingDamageParameters damageParams, ICoreGameManagers coreGameManagers)
        {
            var saveManager = coreGameManagers.GetSaveManager();

            int pyreAttack = saveManager.GetDisplayedPyreAttack();
            int pyreNumAttacks = saveManager.GetDisplayedPyreNumAttacks();

            Plugin.Logger.LogInfo($"Pyre attack: {pyreAttack}, Pyre num attacks: {pyreNumAttacks}, Param int: {this.GetParamInt()}");
            return damageParams.damage + pyreAttack * pyreNumAttacks * this.GetParamInt();
        }


        // Token: 0x06000CD8 RID: 3288 RVA: 0x00036B70 File Offset: 0x00034D70
        public override string GetCardTooltipText()
        {
            return base.LocalizeTraitKey("DiscipleClan.Plugin_CardTraitPyreboost_TooltipText");
        }

        // Token: 0x06000CD9 RID: 3289 RVA: 0x00036B7D File Offset: 0x00034D7D
        public override string GetCardTooltipId()
        {
            return base.LocalizeTraitKey("DiscipleClan.Plugin_CardTraitPyreboost");
        }

        // Token: 0x06000CDA RID: 3290 RVA: 0x00036B84 File Offset: 0x00034D84
        public override string GetCardText()
        {
            return base.LocalizeTraitKey("DiscipleClan.Plugin_CardTraitPyreboost_CardText");
        }
    }
}
