using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnMoveLeft : ButtonHold
{
    protected virtual void Update()
    {
        if (!this.isHolding)
        {
            //CharManager.Instance._charController.isLeftBtnHolding = false;
        }
        else
        {
            //CharManager.Instance._charController.isLeftBtnHolding = true;
        }
    }
}
