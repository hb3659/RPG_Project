using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour 가 StateMachine 을 가지고 있도록 사용
// StateMachine 에서 사용할 State 를 구현 (추상화 클래스)
public abstract class State<T>
{
    // State 를 관리하는 StateMachine 에 접근할 수 있도록
    // T : context 에 대한 타입
    protected StateMachine<T> stateMachine;
    // 상태 (소유자) 에 대한 context
    // MonoBehaviour 타입이 들어온다.
    protected T context;
    public State()
    {

    }

    // StateMachine 과 context 를 설정하는 함수
    internal void SetStateMachineAndContext(StateMachine<T> stateMachine, T context)
    {
        this.stateMachine = stateMachine;
        this.context = context;

        // 상태에 대한 초기화 함수 호출
        OnInitialized();
    }

    public virtual void OnInitialized()
    {

    }
    public virtual void OnEnter()
    {

    }
    // 순수 가상함수
    // 반드시 구현해야 한다.
    public abstract void OnUpdate(float deltaTime);
    public virtual void OnExit()
    {

    }
}

// 다른 클래스에서 상속하지 못하도록 (변형이 없도록)
public sealed class StateMachine<T>
{
    private T context;

    // 현재 상태에 대한 변수
    private State<T> currentState;
    // 프로퍼티 추가
    // 외부에서는 할당 불가, 읽기 전용 (readonly)
    public State<T> CurrentState => currentState;

    // 어떠한 상태에서 변경되었는지 확인하기 위한 변수
    private State<T> priviousState;
    public State<T> PriviousState => priviousState;

    // 상태 변화 후 얼마나 시간이 흘렀는지 확인하기 위한 변수
    private float elapsedTimeInState = 0.0f;
    public float ElapsedTimeInState => elapsedTimeInState;

    // Key -> System.Type, Value -> State<T>
    // 상태를 등록하기 위한 딕셔너리
    // State 의 타입과 State 의 인스턴스로 구성되어진다.
    private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

    public StateMachine(T context, State<T> initialState)
    {
        this.context = context;

        // 상태를 초기화
        // 기본 상태 추가
        AddState(initialState);
        // 등록된 상태를 현재 상태로 설정한 후 바로 실행될 수 있도록 한다.
        currentState = initialState;
        currentState.OnEnter();
    }

    public void AddState(State<T> state)
    {
        // 등록하려는 상태와 가상머신을 가지고 있는 소유자를 파라미터로
        state.SetStateMachineAndContext(this, context);
        // 딕셔너리에 등록
        states[state.GetType()] = state;
    }

    public void Update(float deltaTime)
    {
        // 현 상태에서 시간이 얼마나 흘렀는지 파악 가능
        elapsedTimeInState += deltaTime;
        currentState.OnUpdate(deltaTime);
    }

    // R 은 반드시 State<T> 에서 상속받은 타입이어야 한다
    public R ChangeState<R>() where R : State<T>
    {
        // 타입을 확인하기 위함
        var newType = typeof(R);

        // 새로 변환되어야할 상태의 타입과 동일하다면
        // 같은 상태를 두번 호출하는 것이기 때문에 
        if(currentState.GetType() == newType)
        {
            // 상태 변환을 하지 않는다.
            return currentState as R;
        }

        if(currentState != null)
        {
            // 상태가 존재한다면 현 상태를 종료한다.
            currentState.OnExit();
        }

        // 상태를 교체해준다.
        priviousState = currentState;
        currentState = states[newType];
        currentState.OnEnter();
        elapsedTimeInState = 0.0f;

        return currentState as R;
    }
}
