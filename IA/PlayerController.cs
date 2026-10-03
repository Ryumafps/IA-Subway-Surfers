using UnityEngine;
[System.Serializable]
public class PlayerController
{
    [UnityEngine.SerializeField]
    public Player player;
    [UnityEngine.SerializeField]
    public IA ia;

    public PlayerController(Player p, IA a)
    {
        player = p;
        ia = a;
    }

    public void ControlPlayer()
    {
        Output[] outputs = ia.GetOutputs();
        if (outputs[0].GetValue() > 0 && outputs[1].GetValue() > 0)
        {
            if (outputs[0].GetValue() > outputs[1].GetValue())
            {
                player.up = true;
                player.down = false;
            }
            else
            {
                player.down = true;
                player.up = false;
            }

        }
        player.jump = outputs[2].GetValue() > 0;
    }

    private float boolToFloat(bool b)
    {
        if (b)
        {
            return 1;
        }
        return 0;
    }
    public void GetIAInputs()
    {
        ia.SetInputValue(0, boolToFloat(player.game.WayIsBlocked(1)));
        ia.SetInputValue(1, boolToFloat(player.game.WayIsBlocked(2)));
        ia.SetInputValue(2, boolToFloat(player.game.WayIsBlocked(3)));
        ia.SetInputValue(3, boolToFloat(player.position == 1));
        ia.SetInputValue(4, boolToFloat(player.position == 2));
        ia.SetInputValue(5, boolToFloat(player.position == 3));
        ia.SetInputValue(6, boolToFloat(player.jump));
        ia.SetInputValue(7, boolToFloat(player.hurt));
        ia.SetInputValue(8, boolToFloat(player.game.IsPosBlocked(3, Constants.playerPosX + 3)));
        ia.SetInputValue(9, boolToFloat(player.game.IsPosBlocked(2, Constants.playerPosX + 3)));
        ia.SetInputValue(10, boolToFloat(player.game.IsPosBlocked(1, Constants.playerPosX + 3)));
        ia.SetInputValue(11, boolToFloat(player.game.IsPosBlocked(3, Constants.playerPosX + 6)));
        ia.SetInputValue(12, boolToFloat(player.game.IsPosBlocked(2, Constants.playerPosX + 6)));
        ia.SetInputValue(13, boolToFloat(player.game.IsPosBlocked(1, Constants.playerPosX + 6)));
    }

    public void ResetPlayer()
    {
        player.Reset();
    }

    public void ChangeToIA(IA a)
    {
        ia = a;
    }
}
