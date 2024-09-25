Модуль расширения [ApiCodeGenerator](https://github.com/MobileTeleSystems/ApiCodeGenerator) реализующий генерацию кода для библиотеки [Refit](https://github.com/reactiveui/refit)

# Установка
1. Добавьте в проект пакет `ApiCodeGenerator.OpenApi.Refit` и `Refit`
1. Добавьте документ описывающий Api (в формате json или yaml) и одноименный файл nswag ([описание формата](https://github.com/MobileTeleSystems/ApiCodeGenerator#nswag-file-format))
1. В файле nswag выберите генератор `OpenApiToRefitClient`
1. Пересоберите проект

Теперь в проекте будет доступен интерфейс с именем указанным в файле nswag (по умолчанию совпадает с именем файла документа).

Подробнее о других настройках генерации можно прочитать в описании проекта [ApiCodeGenerator](https://github.com/MobileTeleSystems/ApiCodeGenerator)
