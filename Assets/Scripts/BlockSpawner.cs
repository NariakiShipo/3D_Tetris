using UnityEngine;
using System.Collections.Generic;

namespace Tetris3D
{
    public class BlockSpawner : MonoBehaviour
    {
        public Vector3 spawnPosition;
        private Block currentBlock;
        private Dictionary<TetrominoType, Vector3Int[]> tetrominoShapes;
        private Dictionary<TetrominoType, Color> tetrominoColors;

        public enum TetrominoType
        {
            I, O, T, L, J, S, Z
        }

        void Start()
        {
            InitializeTetrominoShapes();
            InitializeTetrominoColors();
            SetSpawnPosition();
        }

        void SetSpawnPosition()
        {
            spawnPosition = new Vector3(4.5f, 18f, 4.5f);
        }

        void InitializeTetrominoShapes()
        {
            tetrominoShapes = new Dictionary<TetrominoType, Vector3Int[]>();
            tetrominoShapes[TetrominoType.I] = new Vector3Int[]
            {
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(0, 2, 0),
                new Vector3Int(0, 3, 0)
            };
            tetrominoShapes[TetrominoType.O] = new Vector3Int[]
            {
                new Vector3Int(0, 0, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(1, 0, 1)
            };
            tetrominoShapes[TetrominoType.T] = new Vector3Int[]
            {
                new Vector3Int(0, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 1, 0)
            };
            tetrominoShapes[TetrominoType.L] = new Vector3Int[]
            {
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(0, 2, 0),
                new Vector3Int(1, 0, 0)
            };
            tetrominoShapes[TetrominoType.J] = new Vector3Int[]
            {
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(0, 2, 0),
                new Vector3Int(-1, 0, 0)
            };
            tetrominoShapes[TetrominoType.S] = new Vector3Int[]
            {
                new Vector3Int(0, 0, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(-1, 0, 1)
            };
            tetrominoShapes[TetrominoType.Z] = new Vector3Int[]
            {
                new Vector3Int(0, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(1, 0, 1)
            };
        }

        void InitializeTetrominoColors()
        {
            tetrominoColors = new Dictionary<TetrominoType, Color>
            {
                { TetrominoType.I, Color.blue },
                { TetrominoType.O, Color.yellow },
                { TetrominoType.T, new Color(0.5f, 0, 0.5f) }, // 紫色
                { TetrominoType.L, new Color(1f, 0.5f, 0f) }, // 橙色
                { TetrominoType.J, Color.cyan },
                { TetrominoType.S, Color.green },
                { TetrominoType.Z, Color.red }
            };
        }

        public void SpawnFirstBlock()
        {
            SpawnNewBlock();
        }

        public void SpawnNewBlock()
        {
            GridManager gridManager = FindFirstObjectByType<GridManager>();
            TetrominoType randomType = GetRandomTetrominoType();
            Vector3Int[] shape = tetrominoShapes[randomType];
            bool canSpawn = true;
            foreach (Vector3Int localPos in shape)
            {
                Vector3 pos = spawnPosition + (Vector3)localPos;
                if ((int)pos.y >= GridManager.height)
                {
                    canSpawn = false;
                    break;
                }
            }
            if (!canSpawn)
            {
                GameManager.Instance.GameOver();
                return;
            }
            currentBlock = CreateBlock(randomType, spawnPosition);
            if (currentBlock == null)
            {
                GameManager.Instance.GameOver();
                return;
            }
            currentBlock.StartFalling();
        }

        TetrominoType GetRandomTetrominoType()
        {
            System.Array values = System.Enum.GetValues(typeof(TetrominoType));
            return (TetrominoType)values.GetValue(Random.Range(0, values.Length));
        }

        Block CreateBlock(TetrominoType type, Vector3 position)
        {
            GameObject blockParent = new GameObject($"Block_{type}");
            blockParent.transform.position = position;
            Block block = blockParent.AddComponent<Block>();
            block.tetrominoType = type;
            Vector3Int[] shape = tetrominoShapes[type];
            List<Transform> cubes = new List<Transform>();
            Color color = tetrominoColors[type];
            foreach (Vector3Int localPos in shape)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.parent = blockParent.transform;
                cube.transform.localPosition = localPos;
                var renderer = cube.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = color;
                }
                cubes.Add(cube.transform);
            }
            block.InitializeBlock(cubes);
            var controller = blockParent.AddComponent<BlockController>();
            controller.Init(this);
            return block;
        }

        public void OnBlockLanded()
        {
            Invoke(nameof(SpawnNewBlock), 0.5f);
        }

        public Block GetCurrentBlock()
        {
            return currentBlock;
        }
    }

    public class Block : MonoBehaviour
    {
        public BlockSpawner.TetrominoType tetrominoType;
        private List<Transform> cubes;
        public void InitializeBlock(List<Transform> cubes)
        {
            this.cubes = cubes;
        }
        public void StartFalling()
        {
            // 可根據需求實作自動下落邏輯
        }
    }
}