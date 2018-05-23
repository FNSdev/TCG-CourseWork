using UnityEngine;
using System.Collections;

public class UpdateManaCrystalsCommand : Command {

    private int TotalMana;
    private int AvailableMana;

    public UpdateManaCrystalsCommand(Player p, int TotalMana, int AvailableMana)
    {
        CommandSender = p;
        this.TotalMana = TotalMana;
        this.AvailableMana = AvailableMana;
    }

    public override void StartCommandExecution()
    {
        CommandSender.PArea.ManaBar.TotalCrystals = TotalMana;
        CommandSender.PArea.ManaBar.AvailableCrystals = AvailableMana;
        CommandExecutionComplete();
    }
}
