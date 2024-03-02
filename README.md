![workflow-status](https://github.com/Cipher-Geist/fireworks-net/actions/build-csharp-artifact.yml/badge.svg)

# fireworks-net

A collection of fully featured Fireworks Algorithm solvers in .net. The algorithms variations included in this library are 

- Fireworks Algorithm (FWA) Ying Tan, Yuanchun Zhu, "Fireworks Algorithms for Optimization", Proc. of International Conference on Swarm Intelligence (ICSI’2010), June 12-15, Beijing, China, 2010. ICSI 2010, Part II, LNCS 6145, pp.355-364, 2010. Springer-Verlag, 2010. - Referred in code as 2010 paper.

- Fireworks Algorithm 2012 (FWA2012) Ying Tan, "Fireworks Algorithm for Optimization", International Journal of Bio-Inspired Computation, 2012, 4(1), 1-10. - Referred in code as 2012 paper.

- Dynamic Fireworks Algorithm (DFWA) S.Q. Zheng, Andreas Janecek, J.Z. Li, and Y. Tan, "Dynamic Search in Fireworks Algorithm", 2014 
 IEEE World Conference on Computational Intelligence (IEEE WCCI'2014) - IEEE Congress on Evolutionary  Computation (CEC'2014), July 07-11, 2014, Beijing International Convention Center (BICC),  Beijing, China, pp. 3222-3229. - Referred in code as 2014 paper.

We will aim to add additional variations of the Fireworks Algorithm (Enhanced FWA etc.).

This intended to provide a simple to use, fully featured, and extensible implementation of the Fireworks Algorithm and builds on the awesome work found in the [Firework.NET](https://github.com/tsimafei-markhel/Fireworks.NET) repository. In addition to .net language upgrades, this implementation provides improved architecture and additional FWA implementations...

## Reference

Implementation is based on the following papers that can be found by the [link](http://www.cil.pku.edu.cn/publications/):
* Ying Tan, Yuanchun Zhu, "Fireworks Algorithms for Optimization", Proc. of International Conference on Swarm Intelligence (ICSI’2010), June 12-15, Beijing, China, 2010. ICSI 2010, Part II, LNCS 6145, pp.355-364, 2010. Springer-Verlag, 2010. - Referred in code as **2010 paper**.
* Y. Pei, S.Q. Zheng, Y. Tan and Hideyuki Takagi, "An Empirical Study on Influence of Approximation Approaches on Enhancing Fireworks Algorithm", IEEE International Conference on System, Man and Cybernetics (SMC 2012), Seoul, Korea. October 14-17, 2012. - Referred in code as **2012 paper**.
* S.Q. Zheng, Andreas Janecek, J.Z. Li, and Y. Tan, "Dynamic Search in Fireworks Algorithm", 2014 
 IEEE World Conference on Computational Intelligence (IEEE WCCI'2014) - IEEE Congress on Evolutionary  Computation (CEC'2014), July 07-11, 2014, Beijing International Convention Center (BICC),  Beijing, China, pp. 3222-3229.

## License

The project is released under the terms of the MIT license. See [LICENSE](LICENSE) file for the complete text.

## Dependencies

The main library project is built using .net core 8.0 and uses the following libraries:

- [MathNet.Numerics](https://www.nuget.org/packages/MathNet.Numerics/)
- [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging/)]
- [StyleCop.Analyzers](https://www.nuget.org/packages/StyleCop.Analyzers/)]

The test project is also built using .net core 8.0 and uses the following libraries:

- [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/)
- [NUnit](https://www.nuget.org/packages/NUnit/)]
- [Moq](https://www.nuget.org/packages/Moq/)]]

Note NuGet packages are not committed to the repository, you need to use Package Restore to get them before the build.