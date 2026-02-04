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
            return damageParams.damage + pyreAttack * pyreNumAttacks * this.GetParamInt();
        }
    }
}
