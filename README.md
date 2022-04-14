# О боте

## Настройка
Преждевсего нужно скачать [ngrok](https://ngrok.com/download) отсюда (необходима регистрация, возможно можно используя данные github). Или

```shell
choco install ngrok
```


В файле */Properties/launchSettings.json* нужно найти раздел `"environmentVariables"` и изменить его следующим образом.

```json
"environmentVariables": {
  "ASPNETCORE_ENVIRONMENT": "Development",
  "TOKEN": "<Token>",
  "HOST_ADDRESS": "<ngrok address>",
  "DATABASE_HOST": "<database host address>",
  "DATABASE_USER": "<database user>",
  "DATABASE_NAME": "<database username>",
  "DATABASE_PASSWORD": "<database password>"
}
```
Все необходимое выдам лично.

## Добавление новых функций
Чтобы добавить функционал создаем новый класс, наследующийся от `BotAction`. Оверрайдим `BotCommand`, метод `Action`, если необходимо, `ExtraDescription`.

После этого в классе `Bot` добавляем в словарь `Actions` новую пару ключ-значение вида `{"command", new ActionClass()}`.

Пример функционала можно посмотреть в `BookAction`.

## Примечания
Планирую разделить таблицы с книгами и пр. Поведение должно остаться прежним, но тогда разные команды звучат логичнее (При желаниии можно будет добавить какие-нибудь столбцы, а не только те, что есть сейчас).