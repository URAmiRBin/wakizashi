using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class Vertex {
        Vector3 _position;
        Vector2 _uv;
        Vector3 _normal;

        bool _hasUV, _hasNormal;

        public Vertex(Vector3 position) {
            _position = position;
            _hasUV = _hasNormal = false;
        }

        public Vertex(Vector3 position, Vector3 normal) {
            _position = position;
            Normal = normal;
        }

        public Vertex(Vector3 position, Vector2 uv) {
            _position = position;
            UV = uv;
        }

        public Vertex(Vector3 position, Vector2 uv, Vector3 normal) {
            _position = position;
            UV = uv;
            Normal = normal;
        }

        public Vector3 Position { get => _position; }

        public Vector2 UV {
            get => _uv;
            set {
                _uv = value;
                _hasUV = true;
            }
        }

        public Vector3 Normal {
            get => _normal;
            set {
                _normal = value;
                _hasNormal = true;
            }
        }
    }
}
