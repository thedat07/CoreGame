using UnityEngine;

namespace DesignPatterns
{
    /*
    Builder Design Pattern là một mẫu thiết kế cho phép bạn xây dựng một đối tượng phức tạp bước by bước. 
    Thông thường, bạn sẽ có một lớp director chịu trách nhiệm hướng dẫn quá trình xây dựng thông qua một lớp builder.
    */

    // Lớp sản phẩm cần xây dựng
    public class Product
    {
        public string PartA { get; set; }
        public string PartB { get; set; }
        public string PartC { get; set; }

        public void ShowInfo()
        {
            Debug.Log($"PartA: {PartA}, PartB: {PartB}, PartC: {PartC}");
        }
    }

    // Builder interface định nghĩa các bước để xây dựng một sản phẩm
    public interface IBuilder
    {
        void BuildPartA();
        void BuildPartB();
        void BuildPartC();
        Product GetResult();
    }

    // Lớp ConcreteBuilder triển khai IBuilder và xây dựng một sản phẩm cụ thể
    public class ConcreteBuilder : IBuilder
    {
        private Product product = new Product();

        public void BuildPartA()
        {
            product.PartA = "PartA built";
        }

        public void BuildPartB()
        {
            product.PartB = "PartB built";
        }

        public void BuildPartC()
        {
            product.PartC = "PartC built";
        }

        public Product GetResult()
        {
            return product;
        }
    }

    // Lớp Director quản lý quá trình xây dựng, nhưng không cần biết chi tiết xây dựng
    public class Director
    {
        private IBuilder builder;

        public Director(IBuilder builder)
        {
            this.builder = builder;
        }

        public void Construct()
        {
            builder.BuildPartA();
            builder.BuildPartB();
            builder.BuildPartC();
        }
    }

    public class ExampleBuildDesignPattern
    {
        public void Example()
        {
            // Sử dụng Builder Design Pattern
            IBuilder builder = new ConcreteBuilder();
            Director director = new Director(builder);

            director.Construct();

            Product product = builder.GetResult();
            product.ShowInfo();
        }
    }
}