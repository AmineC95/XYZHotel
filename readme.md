
![image](https://github.com/AmineC95/XYZHotel/assets/87375880/0863e963-d841-4c94-9d0f-486289624529)


XYZHotel.Domain/
│
├── Entities/                            # Entités représentant les concepts métiers
│   ├── Customer.cs                      # Entité Client
│   ├── Reservation.cs                   # Entité Réservation
│   ├── Room.cs                          # Entité Chambre
│   └── Payment.cs                       # Entité Paiement
│
├── Enums/                               # Énumérations pour les valeurs fixes
│   ├── Currency.cs                      # Devise
│   ├── ReservationStatus.cs             # Statut de la Réservation
│   └── RoomType.cs                      # Type de Chambre
│
├── ValueObjects/                        # Objets de valeur (Value Objects)
│   ├── Email.cs                         # Objet de valeur Email
│   ├── PhoneNumber.cs                   # Objet de valeur Numéro de Téléphone
│   └── Address.cs                       # Objet de valeur Adresse
│
├── Services/                            # Services pour la logique métier
│   ├── ReservationService.cs            # Service pour la gestion des réservations
│   ├── PaymentService.cs                # Service pour la gestion des paiements
│   └── CustomerService.cs               # Service pour la gestion des clients
│
├── Interfaces/                          # Interfaces pour les services
│   ├── IReservationService.cs           # Interface pour ReservationService
│   ├── IPaymentService.cs               # Interface pour PaymentService
│   └── ICustomerService.cs              # Interface pour CustomerService
│
├── DataAccess/                          # Couche d'accès aux données
│   ├── Repositories/                    # Répertoires pour les entités
│   └── DbContext.cs                     # Contexte de base de données (si EF Core est utilisé)
│
└── README.md                            # Documentation du projet


## Ubiquitous Language:

- **Réservation (Booking) :** 
    Un réservation pour une chambre sur une période donnée, effectué par un client.
- **Chambre (Room) :** 
    Une unité d'hébergement dans l'hôtel, caractérisée par un type et un prix.
- **Client (Customer) :** 
    Une personne qui réserve et utilise les services de l'hôtel.
- **Paiement (Payment) :** 
    Transaction financière effectuée par le client pour payer les services de l'hôtel.

#### Bounded Contexts:

1. **Gestion des Réservations :** 
    S'occupe de tout ce qui est lié à la création, la modification, et l'annulation des réservations.
2. **Gestion des Clients :** 
    Gère les informations relatives aux clients, comme leurs détails personnels et historique de réservations.
3. **Gestion des Chambres :** 
    Implique la maintenance des détails des chambres, y compris leur disponibilité, types, et tarification.

#### Context Maps:

- **Gestion des Réservations <-> Gestion des Clients :** 
    Les réservations nécessitent des informations sur le client.
- **Gestion des Réservations <-> Gestion des Chambres :** 
    Les réservations sont liées à la disponibilité et au type des chambres.
- **Gestion des Clients <-> Gestion des Chambres :** 
    Bien que moins directe, il peut y avoir une interaction, par exemple, des offres spéciales basées sur l'historique du client.

#### Core/Supporting/Generic Domains

- **Domaines Principaux (Core Domains) :** 
    Gestion des Réservations - C'est le cœur de votre application.
- **Domaines Secondaires (Supporting Domains) :** 
    Gestion des Clients, Gestion des Chambres - Ils soutiennent le domaine principal mais ne sont pas le focus principal.
- **Domaines Génériques (Generic Domains) :** 
    Services tels que la gestion des paiements, notifications, ou services de conversion de devises - Ces services sont importants mais ne sont pas uniques au domaine d'activité.
