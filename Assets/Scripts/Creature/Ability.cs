using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Ability : MonoBehaviour
{
    [Header ( "Animations" )]
    public GameObject cast;
    public GameObject projectile;
    public GameObject hit;
    public float projectileSpeed = 100;
    public float projectileArc = 0;
    public float damageDelay = 0;

    [Header ( "Battle" )]
    [HideInInspector] public List<Creature> targets = new List<Creature> ();
    public Creature caster;
    public enum triggers { turn, battleStart, turnStart, battleEnd, turnEnd }
    public triggers trigger;

    [Header ( "Stats" )]
    [Range ( -10 , 10 )]
    public int rangedDmg = 0;
    [Range ( 1 , 5 )]
    public int range = 1;

    public abstract void setTargets ( List<BattleZone> allyZones , List<BattleZone> ennemyZones , Creature _caster = null );

    public virtual IEnumerator execute ()
    {
        if ( targets.Count > 0 )
        {
            yield return StartCoroutine ( animationFirstPart () );
            yield return StartCoroutine ( animationSecondPart () );
        }
    }

    protected virtual IEnumerator animationFirstPart ()
    {
        if ( targets.Count == 0 )
            yield break;

        // Caster windup
        yield return StartCoroutine ( caster.animDash ( 5 , 0.025f ) );

        // Cast anim
        if ( cast )
        {
            GameObject castAnim = Instantiate ( cast , caster.transform.position + new Vector3 ( 0 , 0 , -0.05f ) , cast.transform.rotation );
            yield return StartCoroutine ( waitForDestruction ( castAnim ) );
        }

        // Caster jump
        yield return StartCoroutine ( caster.animDash ( 5 , -0.075f ) );
        StartCoroutine ( caster.animDash ( 5 , 0.05f ) );
    }

    protected virtual IEnumerator animationSecondPart ()
    {
        // For each target + Projectile
        foreach ( Creature target in targets )
        {
            yield return StartCoroutine ( attackWindown ( target.transform.position ) );
            target.getHit ();

            // Damages + Effetcs
            target.takeDamages ( rangedDmg );

            yield return new WaitForSeconds ( 0.1f );
        }
        yield return new WaitForSeconds ( 0.2f );
    }

    protected IEnumerator attackWindown ( Vector3 target )
    {
        if ( projectile )
            yield return StartCoroutine ( projectileTravel ( caster.transform.position , target ) );

        // Caster Windown 
        StartCoroutine ( caster.animDash ( 5 , -0.05f ) );

        // Projectile hit + Target hit
        if ( hit )
            Instantiate ( hit , target + new Vector3 ( 0 , 0 , -0.05f ) , hit.transform.rotation );
    }

    protected IEnumerator waitForDestruction ( GameObject go )
    {
        while ( go != null )
            yield return null;
    }

    protected IEnumerator projectileTravel ( Vector3 origin , Vector3 target )
    {
        GameObject projectileAnim = Instantiate ( projectile , origin + new Vector3 ( 0 , 0 , -0.05f ) , projectile.transform.rotation );
        projectileAnim.transform.rotation = Quaternion.Euler ( -90 * caster.zone.X_Scale , 0 , 0 );
        float _progress = 0;
        float distance = Vector3.Distance ( origin , target );
        // This is one divided by the total flight duration, to help convert it to 0-1 progress.
        float _stepScale = ( projectileSpeed / 100 ) / distance;

        while ( Vector3.Distance ( projectileAnim.transform.position , target ) > 0.01f )
        {
            // Increment our progress from 0 at the start, to 1 when we arrive.
            _progress = Mathf.Min ( _progress + Time.deltaTime * _stepScale , 1.0f );

            // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
            float parabola = 1.0f - 4.0f * ( _progress - 0.5f ) * ( _progress - 0.5f );

            // Travel in a straight line from our start position to the target.        
            Vector3 nextPos = Vector3.Lerp ( origin , target , _progress );

            // Then add a vertical arc in excess of this.
            nextPos.y += parabola * projectileArc;

            // Continue as before.
            //projectileAnim.transform.LookAt ( nextPos , transform.forward );
            //projectileAnim.transform.rotation = Quaternion.Euler ( 0 , projectileAnim.transform.rotation.eulerAngles.y , 0 );
            projectileAnim.transform.position = nextPos;
            yield return null;
        }
        yield return null;
        Destroy ( projectileAnim );
    }
}
