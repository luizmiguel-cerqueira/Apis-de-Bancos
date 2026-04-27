Para fazer:
	- Refatorar classes como Utilidade e admUtilizade para criar interfaces e implementar as classes de acordo com as interfaces criadas.
	- Modificar classe User para cada usuário ter um tipo (admin, cliente, etc) e criar métodos específicos para cada tipo de usuário.
	- Adicionar isso ao DB usando Ef
	- Criar uma classe de serviço para lidar com a lógica de negócios relacionada aos usuários, como autenticação, autorização, etc.
	- Criar uma interface Pessoa, que compõe de (ajustável) id, nome, cpf, data nascimento, endereço, telefone, email, role. Para implementar...
	... em outras interfaces/classes como cliente funcionarios etc...
	- Refatorar todas as querys por conta da mudança estrutural do banco de dados.