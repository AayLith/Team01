using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public string displayName;
    [TextArea ( 3 , 10 )]
    public string description;
    public string flavor;
    public int cost;
    public GameObject anim;

    public void useSpell ()
    {
        if ( costs () )
            StartCoroutine ( targeting () );
    }

    protected virtual IEnumerator targeting ()
    {
        SpellConfirm.instance.open ( this );

        while ( !SpellConfirm.instance.done )
            yield return null;

        if ( SpellConfirm.instance.valid )
            StartCoroutine ( execution ( getTargetPos () , getTargetCreatures () ) );
    }

    protected abstract bool costs ();

    protected abstract List<Creature> getTargetCreatures ();

    protected abstract Vector3 getTargetPos ();

    protected abstract void execute ( Creature c );

    protected virtual IEnumerator execution ( Vector3 pos , List<Creature> targets )
    {
        if ( anim )
        {
            GameObject obj = Instantiate ( anim , pos , Quaternion.identity );
            while ( obj != null )
                yield return null;
        }

        foreach ( Creature c in targets )
            execute ( c );

        foreach ( Creature c in targets )
            if ( c.health <= 0 )
            {
                StartCoroutine ( c.animDeath () );
                yield return new WaitForSeconds ( 0.05f );
            }
    }
}
