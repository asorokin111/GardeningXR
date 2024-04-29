using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryCalculator : MonoBehaviour
{
    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    [SerializeField] private Transform _physicsSceneObjects;
    private void Start()
    {

    }
    //public void CreatePhysicsScene()
    //{
    //    _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
    //    _physicsScene = _simulationScene.GetPhysicsScene();
    //    foreach(Transform obj in _physicsSceneObjects)
    //    {
    //        var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
    //        ghostObj.GetComponent<Renderer>().enabled = false;
    //        SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
    //    }
    //}


}
