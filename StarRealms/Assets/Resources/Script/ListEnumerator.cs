namespace Resources.Script
{
    public enum Faction {Rouge, Vert, Jaune, Bleu, All,Neutre}
    public enum Condition{Nothing,Or,Synergie,AutoScrap,TwoBaseOrMore,ForEachSameFaction,NotFound}
    public enum Effect{D,Discard,Draw,G,H,Sabotage,Wormhole,Hinder,Requisition,Scrap,Copy,DiscardToDraw,
        AllShipOneMoreDamage,BaseDestruction,MultiFaction}
    public enum Zone{Hand,DiscardPile,HandAndDiscardPile,Shop,Board,EnnemyBoardBase,Display}
}