public struct DoubleRect {
    private DoubleVec2 size;
    private DoubleVec2 max;
    private DoubleVec2 min;
    private DoubleVec2 center;

    public DoubleRect(DoubleVec2 center, DoubleVec2 size) {
        this.center = center;
        this.size = size;

        this.max = new DoubleVec2(center.x + size.x * 0.5d, center.y + size.y * 0.5d);
        this.min = new DoubleVec2(center.x - size.x * 0.5d, center.y - size.y * 0.5d);
    }

    public DoubleRect(double minX, double maxX, double minY, double maxY) {
        this.max = new DoubleVec2(maxX, maxY);
        this.min = new DoubleVec2(minX, minY);

        this.center = new DoubleVec2((maxX + minX) * 0.5d, (maxY + minY) * 0.5d);
        this.size = new DoubleVec2(maxX - minX, maxY - minY);
    }

    public DoubleVec2 Size {
        get => size;
        set {
            size = value;

            UpdateMinMax();
        }
    }

    public DoubleVec2 Max {
        get => max;
        set {
            max = value;

            UpdateSizeCenter();
        }
    }

    public DoubleVec2 Min {
        get => min;
        set {
            min = value;

            UpdateSizeCenter();
        }
    }

    public DoubleVec2 Center {
        get => center;
        set {
            center = value;

            UpdateMinMax();
        }
    }

    /// <summary>
    /// x => height 1당 width의 비율
    /// y => width 1당 height의 비율
    /// </summary>
    public DoubleVec2 Ratio {
        get {
            return new DoubleVec2(size.x / size.y, size.y / size.x);
        }
    }

    private void UpdateSizeCenter() {
        size = new DoubleVec2(max.x - min.x, max.y - min.y);
        center = (max + min) * 0.5d;
    }

    private void UpdateMinMax() {
        max = new DoubleVec2(center.x + size.x * 0.5d, center.y + size.y * 0.5d);
        min = new DoubleVec2(center.x - size.x * 0.5d, center.y - size.y * 0.5d);
    }
}