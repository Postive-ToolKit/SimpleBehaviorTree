# Simple-Behavior-Tree

Simple-Behavior-Tree는 유니티에서 사용 가능한 간단한 행동 트리입니다.
행동트리의 기본적인 구조가 구현되어 있으며, 사용자가 원하는 행동을 추가하여 사용할 수 있습니다.
행동 트리(Behavior Tree)는 AI 캐릭터의 행동을 관리하는 데 유용한 도구입니다.

이 문서에서는 Simple-Behavior-Tree에 구현된 행동 트리의 기본 구조와 함께 다양한 노드(Node) 타입을 설명하며, Conditional Abort 기능을 포함한 기능들을 간략히 다룹니다.

## 목차

1. [행동 트리(Behavior Tree)란?](#행동-트리behavior-tree란)
2. [노드(Node) 종류](#노드node-종류)
- [Composite Node](#composite-node)
    - [Sequence Node](#sequence-node)
    - [Selector Node](#selector-node)
    - [Parallel Node](#parallel-node)
    - [Probability Node](#probability-node)
    - [Simple Parallel Node](#simple-parallel-node)
- [Decorator Node](#decorator-node)
    - [Invert Node](#invert-node)
    - [Succeeder Node](#succeeder-node)
    - [Repeater Node](#repeater-node)
    - [Wait Success Node](#wait-success-node)
    - [Blackboard Node](#blackboard-node)
    - [Blackboard Based Condition Node](#blackboard-based-condition-node)
- [Action Node](#action-node)
    - [Debug Log Node](#debug-log-node)
    - [Wait Node](#wait-node)
    - [Message Pass Node](#message-pass-node)
    - [Message Remove Node](#message-remove-node)
- [Condition Node](#condition-node)
    - [Message Receive Node](#message-receive-node)
    - [On Off Node](#on-off-node)
3. [Blackboard 기능](#blackboard-기능)
4. [Conditional Abort 기능](#conditional-abort-기능)
5. [행동 트리의 활용 및 확장](#행동-트리의-활용-및-확장)

## 행동 트리(Behavior Tree)란?

행동 트리(Behavior Tree)는 게임 AI에서 널리 사용되는 의사 결정 구조로, 트리 형태로 다양한 행동을 정의할 수 있습니다. 각 노드는 특정한 행동이나 조건을 담당하며, 트리 구조를 통해 복잡한 행동을 단순하고 이해하기 쉽게 관리할 수 있습니다.

### 행동 트리의 주요 구성 요소

1. **노드(Node)**: 행동 트리의 기본 단위로, 각 노드는 특정 행동이나 조건을 나타냅니다.
2. **Composite Node**: 여러 자식 노드를 포함하며, 자식 노드들을 순차적 또는 조건에 따라 실행합니다.
3. **Decorator Node**: 단일 자식 노드를 포함하며, 자식 노드의 실행 결과를 수정하거나 반복 실행합니다.
4. **Action Node**: 특정 행동을 수행하는 노드입니다.
5. **Condition Node**: 특정 조건을 평가하고, 그에 따라 트리의 흐름을 제어합니다.

## 노드(Node) 종류

### Composite Node

Composite Node는 여러 자식 노드를 가지며, 자식 노드들의 실행 순서와 조건을 결정합니다.

#### Sequence Node

- 자식 노드를 순차적으로 실행하며, 하나의 노드라도 실패하면 즉시 실패를 반환합니다. 모든 노드가 성공하면 성공을 반환합니다.

#### Selector Node

- 자식 노드를 순차적으로 실행하며, 하나의 노드가 성공하면 즉시 성공을 반환합니다. 모든 노드가 실패하면 실패를 반환합니다.

#### Parallel Node

- 모든 자식 노드를 동시에 실행하며, 설정된 조건에 따라 성공 또는 실패를 반환합니다.

#### Probability Node

- 자식 노드들에 대한 확률 기반의 선택을 통해 하나의 노드를 실행합니다. 선택된 노드의 결과에 따라 성공 또는 실패를 반환합니다.

#### Simple Parallel Node

- 모든 자식 노드를 병렬로 실행하며, 성공 또는 실패 여부와 상관없이 모든 노드가 종료될 때까지 기다립니다.

### Decorator Node

Decorator Node는 단일 자식 노드를 가지며, 자식 노드의 실행 결과를 수정하거나 추가적인 조건을 제공합니다.

#### Invert Node

- 자식 노드의 성공을 실패로, 실패를 성공으로 반전시킵니다.

#### Succeeder Node

- 자식 노드의 결과에 상관없이 항상 성공을 반환합니다.

#### Repeater Node

- 자식 노드를 지정된 횟수만큼 반복 실행합니다.

#### Wait Success Node

- 자식 노드가 성공할 때까지 대기합니다.

#### Blackboard Node

- 블랙보드(공유 메모리)를 사용해 특정 데이터를 설정하거나 읽어옵니다. 모든 트리에는 하나의 블랙보드가 존재합니다. 데코레이터를 통해 추가 블랙보드를 할당할 경우 해당 블랙보드는 기존 블랙보드의 데이터를 상속받으며 자신의 자식들이 수신한 데이터를 저장할 수 있습니다. 만일 해당 노드 트리의 작업이 완료되면 해당 블랙보드와 데이터는 삭제됩니다. 즉 블랙보드 데코레이터는 데이터의 스코프를 제한하는 역할을 합니다.

#### Blackboard Based Condition Node

- 블랙보드의 데이터에 기반하여 특정 조건을 평가하고, 조건이 충족되면 자식 노드를 실행합니다.

### Action Node

Action Node는 실제로 게임 내에서 특정 행동을 수행하는 노드입니다.

#### Debug Log Node

- 디버그 메시지를 출력합니다.

#### Wait Node

- 지정된 시간 동안 대기합니다.

#### Message Pass Node

- 특정 메시지를 다른 시스템이나 노드에 전달합니다.

#### Message Remove Node

- 특정 메시지를 제거합니다.

### Condition Node

Condition Node는 특정 조건을 평가하고, 결과에 따라 트리의 흐름을 제어합니다.

#### Message Receive Node

- 특정 메시지를 수신했는지 여부를 평가합니다.

#### On Off Node

- On/Off Boolean 값을 통해 성공 또는 실패를 반환합니다.

## Blackboard 기능

Blackboard는 행동 트리에서 공유 메모리의 역할을 하며, 여러 노드 간에 데이터를 공유할 수 있게 합니다. 이를 통해 트리 내의 다양한 노드들이 동일한 데이터를 참조하거나 수정할 수 있습니다.

### Blackboard의 주요 기능

1. **데이터 저장**: 트리의 특정 시점에서 데이터를 저장하고, 다른 노드가 이를 참조할 수 있게 합니다.
2. **데이터 읽기**: 저장된 데이터를 읽어와 조건을 평가하거나 행동을 결정하는 데 사용합니다.
3. **상태 관리**: AI의 현재 상태나 환경 정보를 저장하여, 행동 결정에 반영할 수 있습니다.

### Blackboard Node와의 연계

- **Blackboard Node**: 블랙보드에 데이터를 쓰거나 읽는 기능을 제공합니다. 이 노드를 통해 블랙보드에 특정 값을 저장하거나, 블랙보드에서 값을 가져와 다른 노드에서 사용할 수 있습니다.
- **Blackboard Based Condition Node**: 블랙보드에 저장된 데이터를 조건으로 평가하여, 특정 조건이 충족될 때에만 자식 노드를 실행합니다.

### 예시 사용 시나리오

- **적의 위치 저장**: AI가 적을 발견했을 때, 적의 위치를 블랙보드에 저장하여 다른 행동 노드들이 이를 참조할 수 있도록 합니다.
- **상태 공유**: 게임 내에서 여러 AI가 같은 블랙보드를 참조하여 팀 전략을 세우거나, 공유된 자원을 관리합니다.

## Conditional Abort 기능

Conditional Abort는 Composite Node에서 사용되는 기능으로, 특정 조건이 충족될 때 자식 노드들의 실행을 중단하고 다시 평가합니다. 이는 동적 상황에 대응하여 행동 트리가 보다 유연하게 동작할 수 있도록 도와줍니다.

### Conditional Abort 유형

1. **Self Abort**: Composite Node가 스스로의 상태를 평가하여 조건이 변화하면 자식 노드의 실행을 중단합니다.
2. **Lower Priority Abort**: 트리에서 더 낮은 우선순위를 가진 다른 Composite Node의 실행을 중단시키고 현재 노드를 재평가합니다.

## 행동 트리의 활용 및 확장

이 기본 구조를 바탕으로 다양한 AI 행동을 정의할 수 있으며, 게임 플레이의 복잡한 시나리오를 간단하고 직관적으로 구현할 수 있습니다. 필요한 경우 새로운 노드를 추가하거나 기존 노드를 확장하여 행동 트리를 더욱 발전시킬 수 있습니다.