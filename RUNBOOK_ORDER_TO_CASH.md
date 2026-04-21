RUNBOOK_ORDER_TO_CASH

# Přehled modulu

Order-to-Cash je první funkční obchodní modul systému MiniERP_2.0.

Řeší běžný firemní proces od přijetí zákazníka, přes vytvoření objednávky, rezervaci skladu, vystavení faktury až po evidenci platby.

Modul slouží jako základní ukázka propojení databáze, business logiky a API vrstvy v rámci ERP systému.

---

# Proces modulu

Zákazník -> Objednávka -> Rezervace skladu -> Potvrzení objednávky -> Faktura -> Platba

---

# Zapojené databázové tabulky

- Customers
- Orders
- OrderItems
- Stock
- StockMovements
- Invoices
- InvoiceItems
- Payments

---

# Důležitá pravidla systému

## Draft objednávka

- lze upravovat
- lze smazat pokud nemá návaznosti

## Confirmed objednávka

- nelze ručně změnit status
- nelze změnit měnu
- lze upravit pouze poznámku a termín

## Fakturace

- fakturu lze vytvořit pouze z Confirmed objednávky
- objednávka musí existovat
- objednávka musí obsahovat položky
- nelze vytvořit duplicitní fakturu

## Platby

- systém nepovolí přeplatek
- jedna faktura může mít více plateb
- po plné úhradě se faktura označí jako Paid

---

# Swagger demo scénář

## Krok 1 – Vytvoření objednávky

POST /api/orders

Očekávání:

- vznikne nová objednávka ve stavu Draft
- dopočítají se částky objednávky

---

## Krok 2 – Rezervace skladu

POST /api/orders/{id}/reserve-stock

Očekávání:

- objednávka přejde do stavu Confirmed
- vytvoří se auditní záznam ve StockMovements

---

## Krok 3 – Vytvoření faktury

POST /api/invoices/from-order/{id}

Očekávání:

- vznikne faktura navázaná na objednávku

---

## Krok 4 – Registrace platby

POST /api/payments

Očekávání:

- platba se uloží
- při plné úhradě se faktura označí jako Paid

---

## Krok 5 – Kontrola výsledku

GET /api/invoices/{id}

Očekávání:

Status = Paid

---

# Očekávané výsledky

Po úspěšném průchodu scénářem by měl systém obsahovat:

- novou objednávku
- rezervovaný sklad
- záznam ve StockMovements
- vystavenou fakturu
- uloženou platbu
- fakturu ve stavu Paid

---

# Aktuální stav

Modul je funkční a testovaný přes Swagger.

Obsahuje kompletní backendový Order-to-Cash proces v první produkční podobě.

---

# Další rozvoj (připravuji)

- storno objednávek
- refundace plateb
- reporting faktur
- dashboard přehledy
- webové UI
- desktop klient
