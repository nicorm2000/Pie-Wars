using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace Pie_Wars.Logic.IA
{
    public enum state
    {
        min =-1,
        idle,
        //----
        //make pie

        ThinkMakePie,
        moveToFindPlateAtConteiner,
        moveToLeavePlateOnFreeCounter,
        moveToFindItemMasa,
        moveToLeaveMasaAtPlate,
        moveToFindItemEspecial,
        moveToLeaveEspecialAtPlate,
        moveToCookPie,
        waitingCooking, //dance
        moveToHeven,
        moveToLeaveThePieInCounter,
        //----

        // atak enemi.
        GoToGetPie,
        MoveToAtak,
        Atak,

        // others.
        avoid,
        stun,
        max
    }
    public enum flags
    {
        min = -1,
        onIdle,
        //avoid
        onProyectyleEnter,
        onProyectyleExit,

        //iddle
        onGoToIddle,
    
        onGoToFindPlateAtConteiner,
        onGetPlate,
        onMoveToLeavePlateInFreeCounter,
        onPutPlateInFreeCounter,
        onGoToFindMasa,
        onTakeMasa,
        onMoveToFindItemSpecial,
        onTakeSpecialIngredient,
        onMoveToFindHeven,
        onPutCakeOnHeven,
        onWaitToCook,
        onGetRedyPie,
        onFindFreeCounter,
        onPutReadyPaiOnCounter,
        //--

        //fight
        onFindPie,
        onGetPie,
        onMoveToAttak,
        onAttak,
        //--


        max
    }

    public enum Order
    {
        cook,
        attack,
    }

    public class Bot : MonoBehaviour , IIngredientObjectParent
    {
        [SerializeField] FSM.FSM fsm = new FSM.FSM();

        //[SerializeField] flags currentFlag = flags.onIdle;

        [SerializeField] float speed = 1;

        [SerializeField] float interactDistance = 1;


        //[SerializeField] Order order = Order.cook;

        //[SerializeField] Transform placeToGo = null;

        [SerializeField] IngredientsSO masa, especial = null;

        [SerializeField] ContainerCounter[] allContainersCounter = null;
        
        [SerializeField] ClearCounter[] allClearConteiners = null;

        [SerializeField] StoveCounter[] stoves = null;

        [SerializeField] PlatesCounter platesCounter = null;

        private int platesAvailable = 0;


        [SerializeField] BaseCounter targetCounter = null;

        [SerializeField] Transform ingredientHoldPoint;

        private IngredientObject ingredientObject;

        public event EventHandler OnPickSomething;

        public PlateObject ActualPlato = null;

        public LayerMask collidersNotPass = new LayerMask();
        private void Start()
        {
            platesCounter.OnPlateSpawned += (a,b) => { platesAvailable++; };
            platesCounter.OnPlateRemoved += (a,b) => { platesAvailable--; };

            void GetPlateConteiner()
            {
                targetCounter = platesCounter;
            }

            void GetContainerCounter(string name)
            {

                for (int i = 0; i < allContainersCounter.Length; i++)
                {
                    if (allContainersCounter[i].ingredient.objectName == name)
                    {
                        targetCounter = allContainersCounter[i];
                        break;
                    }
                }
            }

            void GetContainerHeven()
            {
                targetCounter = stoves.FirstOrDefault();
            }
            
            ClearCounter GetSomeFreeClearConteiner()
            {
                for (int i = 0; i < allClearConteiners.Length; i++)
                {
                    if (!allClearConteiners[i].HasIngredientObject())
                    {
                        return allClearConteiners[i];
                    }
                }
                return null;
            }
            
            fsm.Init((int)state.max, (int)flags.max);

            //pasar de iddle a otro.
            void idleStart()
            {
                if (HasIngredientObject())
                {
                    ClearIngredientObject();
                }
                fsm.SetFlag(flags.onGoToFindPlateAtConteiner);
            }

            fsm.AddState(state.idle, idleStart);

            fsm.SetRelation(state.idle, flags.onGoToFindPlateAtConteiner, state.moveToFindPlateAtConteiner);
            //buscar plato en el contenedor.

            fsm.AddState(state.moveToFindPlateAtConteiner,
                ()=> GetPlateConteiner(),
            () => { if (platesAvailable > 0) MoveUpdate(targetCounter.transform, flags.onGetPlate); },
                ()=> { targetCounter.Interact(this); ActualPlato = GetIngredientObject() as PlateObject; });

            fsm.SetRelation(state.moveToFindPlateAtConteiner, flags.onGetPlate, state.moveToLeavePlateOnFreeCounter);
            //deja el plato en el contenedor.
            fsm.AddState(state.moveToLeavePlateOnFreeCounter,
                () => targetCounter = GetSomeFreeClearConteiner(),
                () => MoveUpdate(targetCounter.transform, flags.onPutPlateInFreeCounter),
                () => targetCounter.Interact(this));

            fsm.SetRelation(state.moveToLeavePlateOnFreeCounter, flags.onPutPlateInFreeCounter, state.moveToFindItemMasa);
            //busca la masa.
            fsm.AddState(state.moveToFindItemMasa,
                () => GetContainerCounter(masa.objectName),
                () => MoveUpdate(targetCounter.transform, flags.onTakeMasa),
                () => targetCounter.Interact(this));

            fsm.SetRelation(state.moveToFindItemMasa, flags.onTakeMasa, state.moveToLeaveMasaAtPlate);
            //busca el plato donde dejar la masa.
            void StartFindPlato(string item)
            {
                if (ActualPlato == null)
                {
                    fsm.SetFlag(flags.onGoToIddle);
                    return;
                }
                for (int i = 0; i < allClearConteiners.Length; i++)
                {
                    if (allClearConteiners[i].GetIngredientObject() == ActualPlato)
                    {
                        targetCounter = allClearConteiners[i];
                        Debug.Log("voy a dejar algo " + item + " en"+ ActualPlato);
                        return;
                    }

                }


                for (int i = 0; i < ActualPlato.ingredientObjectSOList.Count; i++)
                {
                    if(ActualPlato.ingredientObjectSOList[i].objectName == item)
                    {
                        fsm.SetFlag(flags.onGoToIddle);
                        return;
                    }
                }
            }

            fsm.AddState(state.moveToLeaveMasaAtPlate,
                () => { StartFindPlato(masa.objectName); },
                () => MoveUpdate(ActualPlato.transform, flags.onMoveToFindItemSpecial),
                () => targetCounter.Interact(this));

            fsm.SetRelation(state.moveToLeaveMasaAtPlate, flags.onMoveToFindItemSpecial, state.moveToFindItemEspecial);
            //busca la especia.
            fsm.AddState(state.moveToFindItemEspecial,
                () => GetContainerCounter(especial.objectName),
                () => MoveUpdate(targetCounter.transform, flags.onTakeSpecialIngredient),
                () => targetCounter.Interact(this));

            fsm.SetRelation(state.moveToFindItemEspecial, flags.onTakeSpecialIngredient, state.moveToLeaveEspecialAtPlate);

            //busca el plato donde dejar la especia.

            fsm.AddState(state.moveToLeaveEspecialAtPlate,
                () => StartFindPlato(especial.objectName),
                () => MoveUpdate(ActualPlato?.transform, flags.onMoveToFindHeven),
                () => {
                targetCounter.Interact(this);
                targetCounter.Interact(this);
                });

            fsm.SetRelation(state.moveToLeaveEspecialAtPlate, flags.onMoveToFindHeven, state.moveToHeven);

            //con el plato y ya todo armado en la mano, voy al horno

            fsm.AddState(state.moveToHeven,
                () => GetContainerHeven(),
                () => MoveUpdate(targetCounter.transform, flags.onPutCakeOnHeven),
                () => {
                    targetCounter.Interact(this);
                });
            //esperar que se cocine.
            fsm.SetRelation(state.moveToHeven, flags.onPutCakeOnHeven, state.waitingCooking);
            void Dance_and_waite_cook()
            {
                transform.Rotate(Vector3.up, 2);
                StoveCounter hornillo = targetCounter as StoveCounter;

                if (hornillo.IsIddle())
                    fsm.SetFlag(flags.onIdle);

                if ((hornillo).IsCooked())
                {
                    targetCounter.Interact(this);
                    fsm.SetFlag(flags.onGetRedyPie);
                }
            }
            fsm.AddState(state.waitingCooking,
                ()=> { },
                () => Dance_and_waite_cook(),
                () => { }
                );

            //tirar el pie.
            fsm.SetRelation(state.waitingCooking, flags.onGetRedyPie, state.moveToLeaveThePieInCounter);
            fsm.SetRelation(state.waitingCooking, flags.onIdle, state.idle);
            
            fsm.AddState(state.moveToLeaveThePieInCounter,
                () => targetCounter = GetSomeFreeClearConteiner(),
                () => MoveUpdate(targetCounter.transform, flags.onIdle),
                () => { targetCounter.Interact(this); }
                );

            fsm.SetRelation(state.moveToLeaveThePieInCounter, flags.onIdle, state.idle);

            void MoveUpdate(Transform where,flags endflag)
            {
                Transform self = this.transform;
                Vector3 target = new Vector3(where.position.x,0,where.position.z);

                target.y = 0;

                self.LookAt(target);

                Vector3 frontFrw = transform.position;

                if (Physics.CheckBox(frontFrw,Vector3.one * 0.5f,Quaternion.identity, collidersNotPass))
                {
                    Vector3 frontFrwR = transform.position  + transform.right;
                
                    Vector3 frontFrwL = transform.position  - transform.right;
                
                    if (Physics.CheckBox(frontFrwR, Vector3.one * 0.5f,Quaternion.identity, collidersNotPass))
                    {
                        self.LookAt(frontFrwL);
                    }
                    else if (Physics.CheckBox(frontFrwL, Vector3.one * 0.5f,Quaternion.identity, collidersNotPass))
                    {
                        self.LookAt(frontFrwR);
                    }
                }

                Vector3 sum = new Vector3(transform.forward.x, 0, transform.forward.z);

                transform.position += sum * speed * Time.deltaTime;

                if (Vector3.Distance(self.position, target) < interactDistance)
                {
                    Debug.Log("llege a " + where.name + " y ahora me setee en: " + endflag.ToString()); ; fsm.SetFlag(endflag);
                }
            }

            fsm.SetCurrentStateForced(state.idle);

            if (HasIngredientObject())
            {
                ClearIngredientObject();
            }
            fsm.SetFlag(flags.onGoToFindPlateAtConteiner);
        }
        private void Update()
        {
            fsm.Update();
        }

        public Transform GetIngredientObjectFollowTranform()
        {
            return ingredientHoldPoint;
        }

        public void SetIngredientObject(IngredientObject ingredientObject)
        {
            this.ingredientObject = ingredientObject;

            if (ingredientObject != null)
            {
                OnPickSomething?.Invoke(this, EventArgs.Empty);
            }
        }

        public IngredientObject GetIngredientObject()
        {
            return ingredientObject;
        }

        public void ClearIngredientObject()
        {
            ingredientObject = null;
        }

        public bool HasIngredientObject()
        {
            return ingredientObject != null;
        }
    }
}

namespace Pie_Wars.Logic.IA.FSM
{
    public class State
    {
        public Action Update = null;
        public Action Start = null;
        public Action End = null;
    }
    [Serializable]
    public class FSM
    {
        public state currentStateIndex = 0;
        Dictionary<state, State> states = new Dictionary<state, State>();
        private state[,] relations = null;

        public void Init(int states, int flags)
        {
            currentStateIndex = state.min;
            relations = new state[states, flags];
            for (int i = 0; i < states; i++)
            {
                for (int j = 0; j < flags; j++)
                {
                    relations[i, j] = state.min;
                }
            }
        }

        public void SetCurrentStateForced(state state)
        {
            currentStateIndex = state;
        }

        public void SetRelation(state sourceState, flags flag, state destinationState)
        {
            relations[(int)sourceState, (int)flag] = destinationState;
        }

        public void SetFlag(flags flag)
        {
            if (relations[(int)currentStateIndex,(int) flag] != state.min)
            {
                states[currentStateIndex].End?.Invoke();

                currentStateIndex = relations[(int)currentStateIndex,(int) flag];

                states[currentStateIndex].Start?.Invoke();
            }
        }

        public void AddState(state stateIndex, 
            Action onStart = null,
            Action onUpdate = null,
            Action onEnd = null)
        {
            if (!states.ContainsKey(stateIndex))
            {
                State newState = new State();
                newState.Start += onStart;
                newState.Update += onUpdate;
                newState.End += onEnd;

                states.Add(stateIndex, newState);
            }
        }

        public void Update()
        {
            if (states.ContainsKey(currentStateIndex))
            {
                states[currentStateIndex].Update?.Invoke();
            }
        }
    }
}
