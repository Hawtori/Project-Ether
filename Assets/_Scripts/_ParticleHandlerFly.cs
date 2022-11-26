using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlyweightPattern
{

    //Incomplete, particle system threw a hard curve ball with accessing properties.
    public class _ParticleHandlerFly : MonoBehaviour
    {

      List<FlyweightPattern.bulletParticle> allBulletParticles = new List<bulletParticle>();

        //Particle system doesn't like assignment nor declaration
        List<ParticleSystem.MainModule> mainMod;
        List<ParticleSystem.EmissionModule> emissionMod;
        List<ParticleSystem.ShapeModule> shapeMod;
        List<ParticleSystem.CollisionModule> collisionMod;



        // Start is called before the first frame update
        void Start()
        {

            for(int i = 0; i < 100; i++)
            {
            


            }


        }


        void getBulletParticleProperties()
        {
            
        }


        // Update is called once per frame
        void Update()
        {
            
        }
    }

}


