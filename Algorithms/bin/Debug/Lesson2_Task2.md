Предлагается 3 реализации алгоритма бинарного поиска.
Одна - **нормальная** - моя реализация.
Две другие из них: рекурсивная и итеративная - найденные в 
интренете и использованные, практически без изменения.
Как видно из реализаций - мой алгоритм по сути является 
аналогом предложенного в интернете итеративного.

О сложности: если N = array.Length, то:
Сложность Рекурсивного алгоритма: O(Log2(N))
Сложность Итеративного алгоритма: O(Log2(N))
Сложность Нормлаьного алгоритма: O(Log2(N))

Как видно сложности всех вариантов O(Log2(N)). Что и должно 
быть, так как во всех алгоритмах используется прием деления 
пополам, таким образом после K-шагов размер оставшейся части 
будет N/(2^K). Естественно, если N/(2^K) <= 1, то мы элемент 
нашли, или не нашли (так как остался единственный вариант), 
что означает число шагов 2^K ~ N или K ~ Log2(N). Что и 
требовалось доказать.